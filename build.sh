#!/bin/zsh

docker kill Nosted

docker image build -t Nosted .

docker container run --rm -it -d --name Nosted --publish 80:80 Nosted

echo
echo "Link: http://localhost:80/"
echo