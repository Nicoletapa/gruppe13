# gruppe13
Gruppen har jobbed med programmering i sprint 1, 2 og 3, og progresjonen har gått ganske greit. gruppen har da jobbet  jobba med å koble til database, implementere xss, koble til docker og lage sjekkliste. sjekklisten har gruppen hatt problemer med, fordi den vil ikke koble til databasen og overføre data. Derfor så er ikke den funksjonen av selve applikasjon helt ferdig, som gruppen skal jobbe videre på på  i sprint 3. Det gruppen planlegger å gjøre videre er å lage en ansattliste, fikse roller (mekaniker, admin og kunde), og ha en statusfunksjon, men også prøve å bli ferdig med aturiasjon, pålogging, hente og lagre data fra database og dokumentasjon fram til neste og siste innlevering. 


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
