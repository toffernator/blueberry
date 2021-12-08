FROM ubuntu:20.04
RUN apt update && apt install -y openssl
WORKDIR /workdir
COPY entrypoint.sh .
ENTRYPOINT [ "/bin/bash", "entrypoint.sh" ]