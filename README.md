# [SynoEncMnt](https://github.com/duggerd/SynoEncMnt)

Windows utility for mounting and unmounting encrypted shares on a Synology NAS. Can be run by backup software in pre/post commands to mount/dismount backup destination. Enters key in a way so that it will not be saved in the shell history of the NAS. Checks SSH fingerprint of NAS before authenticating and supplying decryption key.

Requires .NET Framework 4.7.2 or later compatible

Tested with a Synology DS118 NAS (DSM 6.2.1).

Download: [latest release](https://github.com/duggerd/SynoEncMnt/releases)

Based on work from [this](https://forum.synology.com/enu/viewtopic.php?f=90&t=107091) Synology forum thread.

Usage
-----

SynoEncMnt.exe [arguments]

Returns 0 for mount/dismount success and 1 for mount/dismount failure.

Arguments:

* mount
* dismount
* config [xml config file path]

Examples:

C:\SynoEncMnt\SynoEncMnt.exe -mount -config C:\bkpcfg\nas_backup1.xml

C:\SynoEncMnt\SynoEncMnt.exe -dismount -config C:\bkpcfg\nas_backup1.xml

Logging:

By default errors are sent to the log file:

%ProgramData%\SynoEncMnt\log.txt

NAS Configuration
-----------------

* create share
* create admin user (mount/dismount requires sudo permissions)
* enable home directories
* enable ssh
* copy mount.sh and dismount.sh to admin user home directory
* chmod +x mount.sh
* chmod +x dismount.sh

Application Configuration
-------------------------

example_config.xml is a template configuration file. All fields are required.

* host: fqdn/ip of nas
* fingerprint: ssh fingerprint
* username: ssh username
* password: ssh password
* share: share to mount/unmount
* key: share decryption key (text key, not key file created on share creation)

To get the fingerprint, leave the placeholder fingerprint and run the utility and take the fingerprint from the log file.

Changelog
---------

* [1.1.0.0 (2019-07-05)](https://github.com/duggerd/SynoEncMnt/releases/tag/v1.1.0.0)

    * [BREAKING CHANGE] Use Mono.Options for argument parsing
    * Update NLog version from 4.6.2 to 4.6.5

* [1.0.0.0 (2019-05-11)](https://github.com/duggerd/SynoEncMnt/releases/tag/v1.0.0.0)

    * Initial release

License
-------

SynoEncMnt is distributed under the MIT License (see LICENSE.txt).

Included open-source components (see doc/SW-LICENSE.txt):

* [Mono.Options](https://github.com/mono/mono/blob/master/mcs/class/Mono.Options/Mono.Options/Options.cs) - MIT License
* [NLog](https://github.com/NLog/NLog) - MIT License
* [SSH.NET](https://github.com/sshnet/SSH.NET) - MIT License

TODO
----

* Remove fixed delay in shell interaction
* Shut down NAS after dismount (option specified with flag)
* Installer (WiX)
