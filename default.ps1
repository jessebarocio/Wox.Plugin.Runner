# psake Build Script

properties {
	$solutionName = "Wox.Plugin.Runner"
	
	$baseDir = Resolve-Path .
	$slnFile = "$baseDir\$solutionName.sln"
	
	$nunitDir = ".\packages\NUnit.Runners.2.6.3\tools"
	$openCoverDir = ".\packages\OpenCover.4.5.2506"
	$reportGeneratorDir = ".\packages\ReportGenerator.1.9.1.0"
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

Task Package -depends Release {
    $releasePath = "$baseDir\Wox.Plugin.Runner\bin\Release"
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