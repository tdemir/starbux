--Create postgresql and redis
--PostgreSql
-Create "starbux" database
-Execute dbScript.sql
--SetUp connectionSettings in appsettings.json and appsettings.Development.json files
--Build docker image
docker build -t starbux_api .
--Run docker container
docker run --rm -it --name starbux_api_container -p 5113:5113 starbux_api
