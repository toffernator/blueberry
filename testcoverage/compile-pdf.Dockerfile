FROM ubuntu:20.04
RUN apt update && DEBIAN_FRONTEND=noninteractive apt install -y wkhtmltopdf
ENTRYPOINT [ "wkhtmltopdf" ]