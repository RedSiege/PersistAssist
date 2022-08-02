# PersistAssist

PersistAssist is a fully modular persistence framework written in C#. All persistence techniques contain a cleanup method which will server to remove the persistence aside from the persistence code. This is a WIP so there are many empty classes, the main object of this project initially was to build out a fully modular framework meant to make adding new features as simple as inheriting a class and adding the code.

Author - Victor Suarez (@Grimmie)

## Compiling 

PersistAssist will not have pre-compiled binaries so you'll have to compile the code manually. To do this open up the solution file and select "Release" and the desired arch in the build menu and hit start.

## Usage

To view the help menu, use `PersistAssist.exe -h`
```
  -t, --technique=VALUE      Persistence technique to use
  -a, --action=VALUE         Action to perform
  -s, --search=VALUE         Keyword to search for
      --rk, --rootkey=VALUE  Root key for registry operations
      --sk, --subkey=VALUE   Sub key for registry operations
      --kv, --keyvalue=VALUE Value to assign regirsty key
      --tn, --taskname=VALUE Task name to set for MSBuild operations
      --pl, --payload=VALUE  Payload to substitute into template
      --fp, --filepath=VALUE Path to file/directory to target
      --dp, --duplicatepath=VALUE
                             Path to duplicate file times from, modified all
                               timestamps
      --ts, --timestamp=VALUE
                             Specify M(odified), A(ccessed), or C(reated)
                               timestamp. Use ALL to target all timestamps
      --nt, --newtime=VALUE  Specify a new date to change specified timestamp to
  -p, --persist              Execute specified techique
  -c, --cleanup              Clean up specified technique
  -l, --list                 List available techniques
  -i, --info                 Displays information on a specified technique
  -h, --help                 show this message and exit
```

To list the available modules, use `PersistAssist.exe -l`
```
[*] Available modules:

Persistence:
============
Registry:
        GenericRegAdd - Add any arbitrary registry key
        RunKeys - Registers a RunKey on either HKLM or HKCU
MSBuild:
        InlineTasks - Deploys MSBuild InlineTask based payload. Drops file to disk
        OverrideTask - Deploys MSBuild OverrideTask based persistence. Drops file to disk and requires admin access
AccountOperations:
Misc:
        PSProfile - Backdoors PowerShell profile files
        StartupFolder - Drops a shortcut to a startup path

Tradecraft:
===========
        SvcList - Lists services on a machine
        Creds - Cred operations
        RegList - Lists contents of specified registry key
        SchList - Lists scheduled tasks on a machine
        TimeStomp - Modifies file and directory time stamps. Does not modify Entry timestamp

Payloads:
=========
        HelloWorld - hola mundo
        PopCalc - Pops calc
```
Persistence contains that available techniques that can be used, the tradecraft modules serve as utility functions to perform various operations, and the payload modules contain paylaods that can be used for various persistence tasks though are currently only available for the OverrideTask module.

To return information on a module, use `PersistAssist.exe -t [technique] -i`
```
Name:     OverrideTask
Desc:     Deploys MSBuild OverrideTask based persistence. Drops file to disk and requires admin access
Usage:
        Persist: PersistAssist.exe -t OverrideTask -p -tn [task name] -pl [payload]
        Cleanup: PeristAssist.exe -t OverrideTask -c -tn [task name]
Category: MSBuild
Author:
```
