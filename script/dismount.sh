/usr/syno/sbin/synoshare --enc_unmount $1
if [ $? -eq 0 ]
then
	echo "EncDismountOk"
	exit 0
else
	echo "EncDismountFailed"
	exit $?
fi
