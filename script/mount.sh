echo -n "EncMountKey: "
read -s passwordVariable
echo ""
/usr/syno/sbin/synoshare --enc_mount $1 $passwordVariable
if [ $? -eq 0 ]
then
	echo "EncMountOk"
	exit 0
else
	echo "EncMountFailed"
	exit $?
fi
