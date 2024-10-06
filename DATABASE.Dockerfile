FROM mcr.microsoft.com/mssql/server:2022-CU13-ubuntu-22.04
USER root
COPY configure-db.sh /usr/config/configure-db.sh
COPY entrypoint.sh /usr/config/entrypoint.sh
COPY setup.sql /usr/config/setup.sql
RUN chown mssql /usr/config
RUN chmod +x /usr/config/entrypoint.sh
RUN chmod +x /usr/config/configure-db.sh

USER mssql
WORKDIR /usr/config

ENTRYPOINT ["./entrypoint.sh"]