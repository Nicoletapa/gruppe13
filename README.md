# gruppe13


**Hvordan koble til database**

1. In a termainal shell:
    - docker pull mariadb
2. Create a data directory for persistent data /my/own/datadir
3. Run: Bash(MAC & LINUX): docker run --rm --name mariadb -p 3308:3306/tcp -v "$(pwd)/database":/var/lib/mysql -e MYSQL_ROOT_PASSWORD=Testingtesting1234 -d mariadb:10.5.11

        Powershell(WINDOWS): docker run --rm --name mariadb -p 3308:3306/tcp -v "%cd%\database":/var/lib/mysql -e MYSQL_ROOT_PASSWORD=12345 -d mariadb:10.5.11
   
5. Access the databse from inside the container:
   - docker exec -it mariadb bash
   - mariadb -u root -p
   - (enter the passoword ) Testingtesting1234

6.  copy in the SQL from CreateDB.sql (line by line).

Server i "Configuration string" i Appsettings.json må endres til "server=172.17.0.1" om det skal kjøres i docker

kjør Nøsted/Dockerfile for å kjøre i docker

kjør kommando docker run -p 80:80 --name Nosted [imageID] 
