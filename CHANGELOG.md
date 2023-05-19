# [v3.6.1] - 2023-5-19 [PR: #51](https://github.com/aksio-insurtech/IngressMiddleware/pull/51)

## Fixed

- Updated Cratis packages to support Aksio.Cratis.Applications.Logging.RenderedCompactJsonFormatter which is the desired format in Aksio cloud-deployed systems.
- Bumped token NuGet packages



# [v3.6.0] - 2023-5-5 [PR: #50](https://github.com/aksio-insurtech/IngressMiddleware/pull/50)

### Added

- Support for converting a JWT bearer token to the Microsoft client principal format and adding it to the `x-ms-client-principal` header on response. This provides us with a consistent way to deal with claims in services / applications.


# [v3.5.1] - 2023-4-25 [PR: #49](https://github.com/aksio-insurtech/IngressMiddleware/pull/49)

### Fixed

- Adding missing principal id and tenant id for when we aren't able to get identity details.


# [v3.5.0] - 2023-4-21 [PR: #48](https://github.com/aksio-insurtech/IngressMiddleware/pull/48)

### Added

- Adding support for OAuth bearer tokens authorization.
- Added `WWW-Authenticate` response headers with error messages and descriptions.

### Fixed

- All errored OAuth Bearer token scenarios now end up with a 401, which seems to be more correct according to RFC7235 (https://www.rfc-editor.org/rfc/rfc7235#section-3.1).


# [v3.4.0] - 2023-4-21 [PR: #47](https://github.com/aksio-insurtech/IngressMiddleware/pull/47)

### Added

- Adding support for OAuth bearer tokens authorization.


# [v3.3.12] - 2023-4-13 [PR: #46](https://github.com/aksio-insurtech/IngressMiddleware/pull/46)

### Fixed

- Fixing encoding for identity details before Base64 encoding it into the target cookie. Browsers can with `atob()` convert ISO-8859-1 encoded strings, UTF-8 strings becomes scrambled when string contains special characters such as Norwegian Æ,Ø,Å.


# [v3.3.11] - 2023-3-29 [PR: #45](https://github.com/aksio-insurtech/IngressMiddleware/pull/45)

### Fixed

- Carrying over the established `Tenant-ID` from the Cratis part of the middleware.


# [v3.3.10] - 2023-3-29 [PR: #44](https://github.com/aksio-insurtech/IngressMiddleware/pull/44)

### Fixed

- Outputting `Tenant-ID`in log for Identity provider call


# [v3.3.9] - 2023-3-29 [PR: #43](https://github.com/aksio-insurtech/IngressMiddleware/pull/43)

### Fixed

- Adding forwarding of `Tenant-ID` header to the identity provider.


# [v3.3.8] - 2023-3-29 [PR: #42](https://github.com/aksio-insurtech/IngressMiddleware/pull/42)

### Fixed

- Removing the requirement of `x-ms-client-principal-id` and `x-ms-client-principal-name` for calling the `.aksio/me` endpoint.

# [v3.3.7] - 2023-3-29 [PR: #41](https://github.com/aksio-insurtech/IngressMiddleware/pull/41)

### Fixed

- Adding log message to see the principal information


# [v3.3.6] - 2023-3-28 [PR: #40](https://github.com/aksio-insurtech/IngressMiddleware/pull/40)

### Fixed

- Removing Serilog `WriteTo` for the base `appsettings.json`. This will allow to override this as Serilog combines sinks from all configuration sources.


# [v3.3.5] - 2023-3-16 [PR: #39](https://github.com/aksio-insurtech/IngressMiddleware/pull/39)

### Fixed

- Removing sensitive data from log output.


# [v3.3.3] - 2023-3-16 [PR: #36](https://github.com/aksio-insurtech/IngressMiddleware/pull/36)

### Fixed

- Adding logging to see what is going on with the identity details provider.


# [v3.3.2] - 2022-12-23 [PR: #35](https://github.com/aksio-insurtech/IngressMiddleware/pull/35)

### Fixed

- The `.aksio-identity` cookie needs to be cleared in the toolbar when we change tenant or identity, since we need to re-evaluate and call the identity details endpoint on the application when that happens.


# [v3.3.1] - 2022-12-23 [PR: #34](https://github.com/aksio-insurtech/IngressMiddleware/pull/34)

### Fixed

- Adding the static files for the toolbar into the development image.


# [v3.3.0] - 2022-12-23 [PR: #33](https://github.com/aksio-insurtech/IngressMiddleware/pull/33)

### Added

- A developer toolbar for easily selecting current tenant and identity.



# [v3.2.1] - 2022-12-22 [PR: #32](https://github.com/aksio-insurtech/IngressMiddleware/pull/32)

### Fixed

- Content of identity details are now a Base64 encoded string. This was an oversight.
- When identity provider returns 403, the entire middleware request will also return 403.



# [v3.2.0] - 2022-12-12 [PR: #31](https://github.com/aksio-insurtech/IngressMiddleware/pull/31)

### Added

- An additional middleware for all requests has been added to be able to call a given URL within an application to resolve identity details based on the incoming Azure app service HTTP headers for identity.



# [v3.1.3] - 2022-12-6 [PR: #30](https://github.com/aksio-insurtech/IngressMiddleware/pull/30)

### Fixed

- Setting tenantId based on host if claims are not present.


# [v3.1.2] - 2022-12-6 [PR: #29](https://github.com/aksio-insurtech/IngressMiddleware/pull/29)

### Fixed

- Fixing config type for Tenant Id claims so that it gets deserialized.


# [v3.1.1] - 2022-12-6 [PR: #28](https://github.com/aksio-insurtech/IngressMiddleware/pull/28)

### Fixed

- Adding logging to see which Tenant-ID gets set for a request.


# [v3.1.0] - 2022-12-6 [PR: #27](https://github.com/aksio-insurtech/IngressMiddleware/pull/27)

### Added

- Adding support for setting Tenant-ID header based on TID claim.



# [v3.0.2] - 2022-11-25 [PR: #23](https://github.com/aksio-insurtech/IngressMiddleware/pull/23)

### Fixed

- Code cleanup.


# [v3.0.1] - 2022-11-25 [PR: #22](https://github.com/aksio-insurtech/IngressMiddleware/pull/22)

### Fixed

- Printing unhandled exceptions.


# [v3.0.0] - 2022-11-25 [PR: #20](https://github.com/aksio-insurtech/IngressMiddleware/pull/20)

### Changed

- Taking a step back and simplifying the flows and not supporting a common **auth** endpoint independent of tenant. Instead letting Container App do most of the heavy lifting and just adding a modified `.well-known` document with correct auth endpoint.


# [v2.1.9] - 2022-11-25 [PR: #18](https://github.com/aksio-insurtech/IngressMiddleware/pull/18)

### Fixed

- Removed deletion of zumo cookie.


# [v2.1.8] - 2022-11-25 [PR: #17](https://github.com/aksio-insurtech/IngressMiddleware/pull/17)

### Fixed

- Configuring origin cookie with more properties for testing.


# [v2.1.7] - 2022-11-25 [PR: #16](https://github.com/aksio-insurtech/IngressMiddleware/pull/16)

### Fixed

- Setting expires on origin cookie and making it an essential cookie.
- URL Decode the content of the origin cookie.


# [v2.1.6] - 2022-11-25 [PR: #15](https://github.com/aksio-insurtech/IngressMiddleware/pull/15)

### Fixed

- Adding logging for origin information passing


# [v2.1.5] - 2022-11-25 [PR: #14](https://github.com/aksio-insurtech/IngressMiddleware/pull/14)

### Fixed

- Changing to cookies for holding which origin the first request is coming from. This is then used when we need to redirect after authentication.


# [v2.1.4] - 2022-11-24 [PR: #13](https://github.com/aksio-insurtech/IngressMiddleware/pull/13)

### Fixed

- Setting the correct callback for the code exchange.


# [v2.1.3] - 2022-11-24 [PR: #12](https://github.com/aksio-insurtech/IngressMiddleware/pull/12)

### Fixed

- Adding Serilog Logging 


# [v2.1.2] - 2022-11-24 [PR: #11](https://github.com/aksio-insurtech/IngressMiddleware/pull/11)

### Fixed

- Adding logging for code exchange


# [v2.1.1] - 2022-11-24 [PR: #10](https://github.com/aksio-insurtech/IngressMiddleware/pull/10)

### Fixed

- Hooking up Azure AD as an OpenID Connect Provider.


# [v2.1.0] - 2022-11-23 [PR: #9](https://github.com/aksio-insurtech/IngressMiddleware/pull/9)

### Added

- Added a way to proxy Azure AD requests and providing a modified `.well-known/openid-configuration` document with authorization url.


# [v2.0.2] - 2022-11-21 [PR: #8](https://github.com/aksio-insurtech/IngressMiddleware/pull/8)

### Fixed

- Making configuration optional.


# [v2.0.1] - 2022-11-16 [PR: #7](https://github.com/aksio-insurtech/IngressMiddleware/pull/7)

### Changed

- Renaming from NginxMiddleware to IngressMiddleware



# [v2.0.0] - 2022-11-16 [PR: #6](https://github.com/aksio-insurtech/IngressMiddleware/pull/6)

### Added

- Introducing support for Norwegian **Id-porten** as an Identity provider with a custom flow to work with Azure Container App.

### Changed

- Configuration file format changed completely.


# [v1.0.2] - 2022-10-18 [PR: #3](https://github.com/aksio-insurtech/NginxMiddleware/pull/3)

### Fixed

- The config path was wrong. Its not looking inside the `config` folder.


# [v1.0.1] - 2022-10-18 [PR: #2](https://github.com/aksio-insurtech/IngressMiddleware/pull/2)

### Fixed

- Fixing wrongly configured docker files and GitHub actions configuration.
