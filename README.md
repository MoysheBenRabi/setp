# Introduction

The goal of this open source project is to define minimal and easy to implement open protocol for networked virtual environments (for example virtual worlds, social 3d environments, online games and first person shooters). Metaverse Exchange Protocol (MXP) is a second generation virtual environment protocol specification. MXP has a coherent theoretical model (Bubble Cloud) for networked virtual environments and proposes a working engineering solution. See [Domain Model](https://github.com/MoysheBenRabi/setp/wiki/Domain-Model) for term definitions. NVEs are in this context systems with multiple thin clients and servers simulating single seamless virtual environment. All parties are upkeeping local partial scene graphs which are synchronized using metaverse exchange protocol. MXP concentrates on messages required for the defined 3d simulation model. MXP specification does not contain application specific concepts but acts as a transport layer for specialized interaction and state data encoded as [Metaverse Structured Data](https://github.com/MoysheBenRabi/setp/wiki/Metaverse-Structured-Data) (MSD).

In addition to MSD/MXP there are the following protocol candidates for authentication, asset and inventory access. These additional protocols leverage HTTP as a transport layer protocol. HTTP access to internet is available in most networks without explicit firewall configurations.

* Asset and inventory access protocol: [OpenSimulator AssetServerProposal](http://opensimulator.org/wiki/AssetServerProposal/ClientDocs)
* Authentication protocol: [OpenId](http://en.wikipedia.org/wiki/Openid)
* Authorization protocol: [OAuth](https://en.wikipedia.org/wiki/OAuth)
