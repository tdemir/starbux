# Create postgresql and redis
## PostgreSql
- Create "starbux" database
- Execute dbScript.sql
- SetUp connectionSettings in appsettings.json and appsettings.Development.json files
# Docker
- Build docker image
```sh
docker build -t starbux_api .
```
- Run docker container
```sh
docker run --rm -it --name starbux_api_container -p 5113:5113 starbux_api
```
