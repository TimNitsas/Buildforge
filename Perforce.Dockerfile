FROM ubuntu:noble

RUN apt-get update
RUN apt-get install curl gpg -y

RUN curl -sS https://package.perforce.com/perforce.pubkey | gpg --dearmor -o /usr/share/keyrings/perforce.gpg
RUN echo "deb [signed-by=/usr/share/keyrings/perforce.gpg] http://package.perforce.com/apt/ubuntu noble release" > /etc/apt/sources.list.d/perforce.list
RUN apt-get update

RUN apt-get install helix-p4d -y
RUN apt-get clean

COPY docker-entrypoint.sh /docker-entrypoint.sh
RUN chmod +x /docker-entrypoint.sh
ENTRYPOINT ["/docker-entrypoint.sh"]