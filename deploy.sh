#!/bin/bash

DEPLOY_HOST_FILE=".deploy_host"

if [ ! -f "$DEPLOY_HOST_FILE" ]; then
  echo "$DEPLOY_HOST_FILE does not exist"
  exit 1
fi

DEPLOY_HOST=`cat $DEPLOY_HOST_FILE`

dotnet build --configuration=Release || exit $?

rsync -r --progress bin/Release/net7.0/ dotnet@$DEPLOY_HOST:/opt/dotnet/netdetective || exit $?

echo "Press enter to restart service, or ^C to exit"
read
ssh -t dotnet@$DEPLOY_HOST -- "sudo systemctl restart netdetective.service" || exit $?

echo Done

