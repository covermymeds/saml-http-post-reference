#C# SAML 2.0 Identity Provider
A reference implementation of a SAML 2.0 identity provider (IdP) and a service provider (SP) to consume the IdP initiated POST binding.

Specification standard [documented here](http://saml.xml.org/wiki/idp-initiated-single-sign-on-post-binding)

This reference Visual Studio 2013 solution includes 4 C# projects:

*	CMMSAMLIdentityProvider - A web application project that provides an interface for gathering user and group attributes and sending it as an IdP initiated HTTP Post. Configurable values include certificate for signing, assertion attributes, Service Provider SSO url and target.
*	CMMSAMLLibrary - Class project for encapsulating common functionality. An XSD.exe generated class for the SAML 2.0 specification is included.
*	CMMSAMLServiceProvider - Web application project that can respond to HTTP POST as an Assertion Consumer Service
*	CMMSAMLSSOTarget - Web application project to serve as the final target for the SAML HTTP POST

This solution and enclosed projects are presented as reference materials form CoverMyMeds only for implementing the oasis SAML 2.0 IdP initiated HTTP POST protocol.
