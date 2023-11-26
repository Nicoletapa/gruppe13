# gruppe13


**Hvordan koble til database**

1. In a termainal shell:
    - docker pull mariadb
2. Create a data directory for persistent data /my/own/datadir
3. Run: docker run --rm --name is201-mariadb -p 127.0.0.1:3306:3306/tcp -v **/my/own/datadir**:/var/lib/mysql -e MYSQL_ROOT_PASSWORD=Testingtesting1234 -d mariadb:latest
4. Access the databse from inside the container:
   - docker exec -it is201-mariadb bash
   - mariadb -u root -p
   - (enter the passoword ) Testingtesting1234

5.  copy in the SQL from CreateDB.sql (line by line).

Server i "Configuration string@2 i Appsettings.json må endres til "server=172.17.0.2" om det skal kjøres i dosker

kjør Nøsted/Dockerfile for å kjøre i docker
