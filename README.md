# Http

![Nuget](https://img.shields.io/nuget/v/Nathiss.Http)

This repository contains the Http project. The Http project contains an implementation of HTTP/1.1 defined by

* [RFC 7230 - Hypertext Transfer Protocol (HTTP/1.1): Message Syntax and Routing](https://tools.ietf.org/html/rfc7230);
* [RFC 7231 - Hypertext Transfer Protocol (HTTP/1.1): Semantics and Content](https://tools.ietf.org/html/rfc7231);
* [RFC 7232 - Hypertext Transfer Protocol (HTTP/1.1): Conditional Requests](https://tools.ietf.org/html/rfc7232);
* [RFC 7233 - Hypertext Transfer Protocol (HTTP/1.1): Range Requests](https://tools.ietf.org/html/rfc7233);
* [RFC 7234 - Hypertext Transfer Protocol (HTTP/1.1): Caching](https://tools.ietf.org/html/rfc7234);
* [RFC 7235 - Hypertext Transfer Protocol (HTTP/1.1): Authentication](https://tools.ietf.org/html/rfc7235);

and an implementation of HTTP/2 defined by

* [RFC 7540 - Hypertext Transfer Protocol Version 2 (HTTP/2)](https://tools.ietf.org/html/rfc7540).

Have in mind that the implementations do NOT implement all HTTP/1.1 and HTTP/2 functionalities.
They implement only must-haves and some other features on the fancy site but only the simple ones.

Although HTTP/1.1 and HTTP/2 differ on transport layer, they share the same schematics (methods, header fields, etc.),
so that's why I decided to put the implementation in one repository.

## Project structure

The implementations of both protocol versions are placed inside [Http/](./Http) directory. In the project root are placed
HTTP schematics shared by both protocol versions.\
Inside [Http1.1/](./Http/Http1.1) directory are placed schematics for HTTP/1.1.\
Inside [Http2/](./Http/Http2) directory are placed schematics for HTTP/2.

## HTTP/1.1

Hypertext Transfer Protocol Version 1.1 is defined by RFCs ranging from 7230 to 7235. The protocol was previously defined
by [RFC 2616](https://tools.ietf.org/html/rfc2616), which is now obsolete.

*Here in the future I'll put the design considerations and design description.*

## HTTP/2

Hypertext Transfer Protocol Version 2 is defined by RFC 7540.

*Here in the future I'll put the design considerations and design description.*

## Notes

This repository was designed to be used as a submodule of [SharpShare](https://github.com/nathiss/SharpShare) repository, thus
is has external dependencies to other components of *SharpShare* (namely [Uri](https://github.com/nathiss/Uri) but in the future
possibly also others). If you wish to use this repository standalone or as a dependency to other project you need to change paths
to the projects that this depends on (see [Http.csproj](Http/Http.csproj)). Take a look at [.travis.yml](.travis.yml) file.

## License

This program is distributed under The MIT License. See [LICENSE.txt](LICENSE.txt) file.
