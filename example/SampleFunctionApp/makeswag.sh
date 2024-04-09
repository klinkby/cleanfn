#!/usr/bin/env bash

# This script generates a Swagger file for the function app
# Prerequisites: func host is installed and a Release is built
# The script starts the function app and waits for the message "For detailed output" to appear
# Once the message appears, the script sends a GET request to the swagger endpoint and saves the 
# response to swagger.json
# The script then kills the function app
# The script is intended to be run from the project root of the function app
PORT=7071
BASEURL="http://localhost:${PORT}/api/"
SWAGGERJSON="swagger.json"
SWAGGERYAML="swagger.yaml"
pushd ./bin/x64/Release/net8.0/linux-x64 || exit
func host start --pause-on-error --dotnet-isolated --functions RenderSwaggerDocument --timeout 10 --no-build --port $PORT | 
tee /dev/tty | {
 grep -q "For detailed output" && {
   popd || exit
   curl -X GET ${BASEURL}${SWAGGERJSON} > ${SWAGGERJSON}
   curl -X GET ${BASEURL}${SWAGGERYAML} > ${SWAGGERYAML}
   kill -HUP -- -`ps x -o  "%r %c" | grep func | grep -o "[[:digit:]]*"`
 }
 cat >/dev/null
}
popd || exit 


