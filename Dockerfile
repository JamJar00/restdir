FROM mcr.microsoft.com/dotnet/runtime:3.1-alpine

COPY ./src/bin/Release/net6.0/linux-x64/publish /restdir

RUN chmod +x /restdir/restdir \
  && apk update \
  && apk add bash

VOLUME [ "/srv" ]
WORKDIR /srv

ENTRYPOINT ["/restdir/restdir"]
