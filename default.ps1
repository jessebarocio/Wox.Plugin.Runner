# psake Build Script

properties {
	$solutionName = "Wox.Plugin.Runner"
	
	$baseDir = Resolve-Path .
	$slnFile = "$baseDir\$solutionName.sln"
	
	$nunitDir = ".\packages\NUnit.Runners.2.6.3\tools"
	$openCoverDir = ".\packages\OpenCover.4.5.2506"
	$reportGeneratorDir = ".\packages\ReportGenerator.1.9.1.0"
        
    $releasePath = "$baseDir\Wox.Plugin.Runner\bin\Release"
}

Task Default -depends Build

Task Build {
    Compile "Debug"
    echo "Build Complete"
}

Task Release {
    Compile "Release"
    echo "Release Complete"
}

Task IncrementVersion {
    $assemblyInfo = "$baseDir\Wox.Plugin.Runner\Properties\AssemblyInfo.cs"
    $rawContent = Get-Content $assemblyInfo -Raw
    $regex = New-Object System.Text.RegularExpressions.Regex("([0-9]+\.){3}[0-9]+")
    $version = $regex.Matches($rawContent)[0].Value
    
    $replaceRegex = New-Object System.Text.RegularExpressions.Regex("(?<=([0-9]+\.[0-9]+\.))[0-9]+(?=\.[0-9])")
    $newVersion = $replaceRegex.Replace($version, ([int]($replaceRegex.Match($version).Value) + 1).ToString())
    
    $rawContent.Replace($version, $newVersion).Trim() | Out-File $assemblyInfo -Encoding UTF8
}

Task GenerateManifest {
    $primaryAssembly = Get-Item "$baseDir\Wox.Plugin.Runner\bin\Release\Wox.Plugin.Runner.dll" | select -ExpandProperty FullName
    
    $assembly = [Reflection.Assembly]::LoadFile($primaryAssembly)
    $manifestTemplate = Get-Content "$baseDir\plugin.json.template" -Raw

    $title = $assembly.GetName().Name
    $description = $assembly.GetCustomAttributes([System.Type]::GetType("System.Reflection.AssemblyDescriptionAttribute"), $false)[0].Description
    $company = $assembly.GetCustomAttributes([System.Type]::GetType("System.Reflection.AssemblyCompanyAttribute"), $false)[0].Company
    $version = $assembly.GetName().Version
    $packageVersion = "$($version.Major).$($version.Minor).$($version.Build)"
    
    $manifestTemplate = $manifestTemplate.Replace("#title#", $title)
    $manifestTemplate = $manifestTemplate.Replace("#description#", $description)
    $manifestTemplate = $manifestTemplate.Replace("#author#", $company)
    $manifestTemplate = $manifestTemplate.Replace("#version#", $packageVersion)
    
    $manifestTemplate | Out-File "$releasePath\plugin.json" -Encoding UTF8
}

Task Package -depends IncrementVersion, Release, GenerateManifest {
    pushd $releasePath
    $packageFiles = ls -Recurse -Exclude Wox.Plugin.dll,*.pdb -Attributes !D
    Zip $packageFiles "$baseDir\Wox.Plugin.Runner.wox"
    popd
}

function Compile( $buildConfig ) {
	# Install solution-level NuGet packages (if there are any)
    if(Test-Path .\.nuget\packages.config) {
	   .\.nuget\nuget.exe install .\.nuget\packages.config -o packages
    }
	# Build
	exec { msbuild /ds $slnFile /p:Configuration=$buildConfig }
}

function Zip($files, $output) {
    $files | Write-Zip -OutputPath $output
}