# NOTICE: I am no longer maintaining this project. Feel free to fork and republish.


# Wox.Plugin.Runner

A plugin that allows you to create simple command shortcuts in [Wox](http://getwox.com).

![Demo](demo.gif)

## Configuration

Configure the plugin in the Wox Settings window. Each item has four settings:

* Description - the human-readable command name that shows up in the Wox command list.
* Shortcut - the shortcut or alias you want to use.
* Path - the path to the program, or script you want to run.
* Arguments - the arguments format to use when launching the program or script. This is a [.NET format string](https://msdn.microsoft.com/en-us/library/txafckwd.aspx).

## Examples

The following sets up a shortcut that opens a remote desktop session:

* Description - `Remote Desktop`
* Shortcut - `rdp`
* Path - `mstsc`
* Arguments - `/v:{0}`

Now all I need to do is type in `r rdp myserver`, hit enter, and a remote desktop session would be launched.
