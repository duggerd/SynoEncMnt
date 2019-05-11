# [SynoEncMnt](https://github.com/duggerd/SynoEncMnt)

Windows utility for mounting and unmounting encrypted shares on a Synology NAS. Can be run by backup software in pre/post commands to mount/dismount backup destination. Enters key in a way so that it will not be saved in the shell history of the NAS. Checks SSH fingerprint of NAS before authenticating and supplying decryption key.

Requires .NET Framework 4.7.2 or later compatible

Tested with a Synology DS118 NAS (DSM 6.2.1).

Download: [latest release](https://github.com/duggerd/SynoEncMnt/releases)

Based on work from [this](https://forum.synology.com/enu/viewtopic.php?f=90&t=107091) Synology forum thread.

Usage
-----

SynoEncMnt.exe [action] [config]

* action: mount/dismount
* config: path to configuration xml file

Returns 0 for mount/dismount success and 1 for mount/dismount failure.

Examples:

C:\SynoEncMnt\SynoEncMnt.exe mount C:\bkpcfg\nas_backup1.xml

C:\SynoEncMnt\SynoEncMnt.exe dismount C:\bkpcfg\nas_backup1.xml

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

To get the fingerprint, leave the placeholder fingerprint and run the utility and take the fingerprint from the log file (strip '-' characters).

Changelog
---------

1.0.0.0 - Initial release

License
-------

SynoEncMnt is distributed under the MIT License.

Included open-source libraries:

* [SSH.NET](https://github.com/sshnet/SSH.NET) - MIT License

TODO
----

* Remove fixed delay in shell interaction
* Shut down NAS after dismount (optional specified in config)
* Installer (WiX)
