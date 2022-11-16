# PersistAssist

PersistAssist is a fully modular persistence framework written in C#. All persistence techniques contain a cleanup method which will server to remove the persistence aside from the persistence code. This is a WIP so there are many empty classes, the main object of this project initially was to build out a fully modular framework meant to make adding new features as simple as inheriting a class and adding the code.

Author - Victor Suarez (@Gr1mmie)

## Compiling 

PersistAssist will not have pre-compiled binaries so you'll have to compile the code manually. To do this open up the solution file and select "Release" and the desired arch in the build menu and hit start.

## Usage

To view the help menu, use `PersistAssist.exe -h`
```
 ______                  __       __   _______             __       __
|   __ .-----.----.-----|__.-----|  |_|   _   .-----.-----|__.-----|  |_
|    __|  -__|   _|__ --|  |__ --|   _|       |__ --|__ --|  |__ --|   _|
|___|  |_____|__| |_____|__|_____|____|___|___|_____|_____|__|_____|____|
   Author: @Grimmie (@FortyNorthSec)
      Ver: v0.2

Usage: PersistAssist.exe -t [technique] -<extra options>
Provide the persist technique and what to do with the technique (persist, cleanup, display info)
To list all available persistence techiques, use PersistAssist.exe -l
  -t, --technique=VALUE      Persistence technique to use
  -a, --action=VALUE         Action to perform
  -s, --search=VALUE         Keyword to search for
      --cmd, --command=VALUE Command to use as payload
      --rk, --rootkey=VALUE  Root key for registry operations
      --sk, --subkey=VALUE   Sub key for registry operations
      --kv, --keyvalue=VALUE Value to assign regirsty key
      --rc, --registrycontext=VALUE
                             Context to write reg key to (options: hkcu or hklm)
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
      --un, --username=VALUE Specify username for credCheck
      --pw, --passwd=VALUE   Specify password for credCheck
      --efq, --eventFilterQuery=VALUE
                             EventFilter query for WMI event subscription
      --efn, --eventFilterName=VALUE
                             EventFilter name for WMI event subscription
      --ecn, --eventConsumerName=VALUE
                             EventConsumer name for WMI event subscription
      --efv, --eventConsumerValue=VALUE
                             EventConsumer value for WMI event subscription
  -q, --query=VALUE          Query to run
      --dn, --domain=VALUE   Specify current domain
  -p, --persist              Execute specified techique
  -c, --cleanup              Clean up specified technique
  -l, --list                 List available techniques
      --lm, --listmodule=VALUE
                             List available techniques from specified module
                               category
  -i, --info                 Displays information on a specified technique
  -h, --help                 show this message and exit
```

To list the available modules, use `PersistAssist.exe -l`
```
 ______                  __       __   _______             __       __
|   __ .-----.----.-----|__.-----|  |_|   _   .-----.-----|__.-----|  |_
|    __|  -__|   _|__ --|  |__ --|   _|       |__ --|__ --|  |__ --|   _|
|___|  |_____|__| |_____|__|_____|____|___|___|_____|_____|__|_____|____|
   Author: @Grimmie (@FortyNorthSec)
      Ver: v0.2

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
WMI:
Misc:
        PSProfile - Backdoors PowerShell profile files
        StartupFolder - Drops a shortcut to a startup path

Tradecraft:
===========
        SvcList - Lists services on a machine
        Creds - Cred operations
        FileRead - Reads a file in memory to get around having to download files for reading
        NetList - basically ipconfig
        ProcList - Lists running processes
        RegList - Lists contents of specified registry key
        SchList - Lists scheduled tasks on a machine
        TimeStomp - Modifies file and directory time stamps. Does not modify Entry timestamp
        WMIQuery - Run an arbitrary WMI Query
        Compile - Standalone utility to compile exes based on C# payloads included in the framework

Payloads:
=========
CSharp:
        HelloWorld - hola mundo
        MsgBox - Displays a MessageBox
        PopCalc - pops calc
        PopCalcAPI - Pops calc via the API
VBA:
```

To list only modules belonging to a specific category, use `-lm`. i.e listing out all the available Tradecraft modules: `PersistAssist.exe -lm Tradecraft`
```
 ______                  __       __   _______             __       __
|   __ .-----.----.-----|__.-----|  |_|   _   .-----.-----|__.-----|  |_
|    __|  -__|   _|__ --|  |__ --|   _|       |__ --|__ --|  |__ --|   _|
|___|  |_____|__| |_____|__|_____|____|___|___|_____|_____|__|_____|____|
   Author: @Grimmie (@FortyNorthSec)
      Ver: v0.2

Tradecraft:
==========
        SvcList - Lists services on a machine
        Creds - Cred operations
        FileRead - Reads a file in memory to get around having to download files for reading
        NetList - basically ipconfig
        ProcList - Lists running processes
        RegList - Lists contents of specified registry key
        SchList - Lists scheduled tasks on a machine
        TimeStomp - Modifies file and directory time stamps. Does not modify Entry timestamp
        WMIQuery - Run an arbitrary WMI Query
        Compile - Standalone utility to compile exes based on C# payloads included in the framework
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
