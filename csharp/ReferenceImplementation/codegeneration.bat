cd MXP\ExtensionFragments
protogen.exe -i:MXP.Common.Proto -o:MetaverseStructuredDataTypes.cs
protogen.exe -i:MXP.Extentions.OpenMetaverseFragments.Proto -o:OpenMetaverseFragments.cs
cd ..\..