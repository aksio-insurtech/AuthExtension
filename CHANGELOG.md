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
