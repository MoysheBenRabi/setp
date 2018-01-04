
      // Generated from 
using MXP.Common.Proto;
namespace MXP.Extentions.OpenMetaverseFragments.Proto
    {
      
    [System.Serializable, ProtoBuf.ProtoContract(Name=@"OmTypeListResponseExt")]
    public partial class OmTypeListResponseExt : ProtoBuf.IExtensible
    {
      public OmTypeListResponseExt() {}
      
    private readonly System.Collections.Generic.List<OmObjectType> _ObjectType = new System.Collections.Generic.List<OmObjectType>();
    [ProtoBuf.ProtoMember(1, Name=@"ObjectType", DataFormat = ProtoBuf.DataFormat.Default)]
    public System.Collections.Generic.List<OmObjectType> ObjectType
    {
      get { return _ObjectType; }
    }
  
      private ProtoBuf.IExtension extensionObject;
      ProtoBuf.IExtension ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
  
    [System.Serializable, ProtoBuf.ProtoContract(Name=@"OmObjectType")]
    public partial class OmObjectType : ProtoBuf.IExtensible
    {
      public OmObjectType() {}
      
    private string _TypeId;
    [ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"TypeId", DataFormat = ProtoBuf.DataFormat.Default)]
    public string TypeId
    {
      get { return _TypeId; }
      set { _TypeId = value; }
    }
    private string _TypeName;
    [ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"TypeName", DataFormat = ProtoBuf.DataFormat.Default)]
    public string TypeName
    {
      get { return _TypeName; }
      set { _TypeName = value; }
    }
      private ProtoBuf.IExtension extensionObject;
      ProtoBuf.IExtension ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
  
    [System.Serializable, ProtoBuf.ProtoContract(Name=@"OmInsertRequestExt")]
    public partial class OmInsertRequestExt : ProtoBuf.IExtensible
    {
      public OmInsertRequestExt() {}
      
    private string _TypeId;
    [ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"TypeId", DataFormat = ProtoBuf.DataFormat.Default)]
    public string TypeId
    {
      get { return _TypeId; }
      set { _TypeId = value; }
    }
    private MsdVector3f _Location;
    [ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"Location", DataFormat = ProtoBuf.DataFormat.Default)]
    public MsdVector3f Location
    {
      get { return _Location; }
      set { _Location = value; }
    }
    private MsdQuaternion4f _Orientation;
    [ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"Orientation", DataFormat = ProtoBuf.DataFormat.Default)]
    public MsdQuaternion4f Orientation
    {
      get { return _Orientation; }
      set { _Orientation = value; }
    }
      private ProtoBuf.IExtension extensionObject;
      ProtoBuf.IExtension ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
  
    [System.Serializable, ProtoBuf.ProtoContract(Name=@"OmInsertResponseExt")]
    public partial class OmInsertResponseExt : ProtoBuf.IExtensible
    {
      public OmInsertResponseExt() {}
      
    private string _ObjectId;
    [ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"ObjectId", DataFormat = ProtoBuf.DataFormat.Default)]
    public string ObjectId
    {
      get { return _ObjectId; }
      set { _ObjectId = value; }
    }
      private ProtoBuf.IExtension extensionObject;
      ProtoBuf.IExtension ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
  
    [System.Serializable, ProtoBuf.ProtoContract(Name=@"OmUpdateRequestExt")]
    public partial class OmUpdateRequestExt : ProtoBuf.IExtensible
    {
      public OmUpdateRequestExt() {}
      
    private string _ObjectId;
    [ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"ObjectId", DataFormat = ProtoBuf.DataFormat.Default)]
    public string ObjectId
    {
      get { return _ObjectId; }
      set { _ObjectId = value; }
    }
    private string _Name;
    [ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"Name", DataFormat = ProtoBuf.DataFormat.Default)]
    public string Name
    {
      get { return _Name; }
      set { _Name = value; }
    }
    private MsdVector3f _Location;
    [ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"Location", DataFormat = ProtoBuf.DataFormat.Default)]
    public MsdVector3f Location
    {
      get { return _Location; }
      set { _Location = value; }
    }
    private MsdQuaternion4f _Orientation;
    [ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"Orientation", DataFormat = ProtoBuf.DataFormat.Default)]
    public MsdQuaternion4f Orientation
    {
      get { return _Orientation; }
      set { _Orientation = value; }
    }
    private float _Scale;
    [ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"Scale", DataFormat = ProtoBuf.DataFormat.FixedSize)]
    public float Scale
    {
      get { return _Scale; }
      set { _Scale = value; }
    }
      private ProtoBuf.IExtension extensionObject;
      ProtoBuf.IExtension ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
  
    [System.Serializable, ProtoBuf.ProtoContract(Name=@"OmDeleteRequestExt")]
    public partial class OmDeleteRequestExt : ProtoBuf.IExtensible
    {
      public OmDeleteRequestExt() {}
      
    private string _ObjectId;
    [ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"ObjectId", DataFormat = ProtoBuf.DataFormat.Default)]
    public string ObjectId
    {
      get { return _ObjectId; }
      set { _ObjectId = value; }
    }
      private ProtoBuf.IExtension extensionObject;
      ProtoBuf.IExtension ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
  
    [System.Serializable, ProtoBuf.ProtoContract(Name=@"OmChatExt")]
    public partial class OmChatExt : ProtoBuf.IExtensible
    {
      public OmChatExt() {}
      
    private string _Message;
    [ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"Message", DataFormat = ProtoBuf.DataFormat.Default)]
    public string Message
    {
      get { return _Message; }
      set { _Message = value; }
    }
      private ProtoBuf.IExtension extensionObject;
      ProtoBuf.IExtension ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
  
    [System.Serializable, ProtoBuf.ProtoContract(Name=@"OmSlPrimitiveExt")]
    public partial class OmSlPrimitiveExt : ProtoBuf.IExtensible
    {
      public OmSlPrimitiveExt() {}
      
    private uint _State;
    [ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"State", DataFormat = ProtoBuf.DataFormat.TwosComplement)]
    public uint State
    {
      get { return _State; }
      set { _State = value; }
    }
    private uint _Material;
    [ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"Material", DataFormat = ProtoBuf.DataFormat.TwosComplement)]
    public uint Material
    {
      get { return _Material; }
      set { _Material = value; }
    }
    private uint _ClickAction;
    [ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"ClickAction", DataFormat = ProtoBuf.DataFormat.TwosComplement)]
    public uint ClickAction
    {
      get { return _ClickAction; }
      set { _ClickAction = value; }
    }
    private MsdVector3f _Scale;
    [ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"Scale", DataFormat = ProtoBuf.DataFormat.Default)]
    public MsdVector3f Scale
    {
      get { return _Scale; }
      set { _Scale = value; }
    }
    private uint _UpdateFlags;
    [ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"UpdateFlags", DataFormat = ProtoBuf.DataFormat.TwosComplement)]
    public uint UpdateFlags
    {
      get { return _UpdateFlags; }
      set { _UpdateFlags = value; }
    }
    private uint _PathCurve;
    [ProtoBuf.ProtoMember(6, IsRequired = true, Name=@"PathCurve", DataFormat = ProtoBuf.DataFormat.TwosComplement)]
    public uint PathCurve
    {
      get { return _PathCurve; }
      set { _PathCurve = value; }
    }
    private uint _ProfileCurve;
    [ProtoBuf.ProtoMember(7, IsRequired = true, Name=@"ProfileCurve", DataFormat = ProtoBuf.DataFormat.TwosComplement)]
    public uint ProfileCurve
    {
      get { return _ProfileCurve; }
      set { _ProfileCurve = value; }
    }
    private uint _PathBegin;
    [ProtoBuf.ProtoMember(8, IsRequired = true, Name=@"PathBegin", DataFormat = ProtoBuf.DataFormat.TwosComplement)]
    public uint PathBegin
    {
      get { return _PathBegin; }
      set { _PathBegin = value; }
    }
    private uint _PathEnd;
    [ProtoBuf.ProtoMember(9, IsRequired = true, Name=@"PathEnd", DataFormat = ProtoBuf.DataFormat.TwosComplement)]
    public uint PathEnd
    {
      get { return _PathEnd; }
      set { _PathEnd = value; }
    }
    private uint _PathScaleX;
    [ProtoBuf.ProtoMember(10, IsRequired = true, Name=@"PathScaleX", DataFormat = ProtoBuf.DataFormat.TwosComplement)]
    public uint PathScaleX
    {
      get { return _PathScaleX; }
      set { _PathScaleX = value; }
    }
    private uint _PathScaleY;
    [ProtoBuf.ProtoMember(11, IsRequired = true, Name=@"PathScaleY", DataFormat = ProtoBuf.DataFormat.TwosComplement)]
    public uint PathScaleY
    {
      get { return _PathScaleY; }
      set { _PathScaleY = value; }
    }
    private uint _PathShearX;
    [ProtoBuf.ProtoMember(12, IsRequired = true, Name=@"PathShearX", DataFormat = ProtoBuf.DataFormat.TwosComplement)]
    public uint PathShearX
    {
      get { return _PathShearX; }
      set { _PathShearX = value; }
    }
    private uint _PathShearY;
    [ProtoBuf.ProtoMember(13, IsRequired = true, Name=@"PathShearY", DataFormat = ProtoBuf.DataFormat.TwosComplement)]
    public uint PathShearY
    {
      get { return _PathShearY; }
      set { _PathShearY = value; }
    }
    private int _PathTwist;
    [ProtoBuf.ProtoMember(14, IsRequired = true, Name=@"PathTwist", DataFormat = ProtoBuf.DataFormat.ZigZag)]
    public int PathTwist
    {
      get { return _PathTwist; }
      set { _PathTwist = value; }
    }
    private int _PathTwistBegin;
    [ProtoBuf.ProtoMember(15, IsRequired = true, Name=@"PathTwistBegin", DataFormat = ProtoBuf.DataFormat.ZigZag)]
    public int PathTwistBegin
    {
      get { return _PathTwistBegin; }
      set { _PathTwistBegin = value; }
    }
    private int _PathRadiusOffset;
    [ProtoBuf.ProtoMember(16, IsRequired = true, Name=@"PathRadiusOffset", DataFormat = ProtoBuf.DataFormat.ZigZag)]
    public int PathRadiusOffset
    {
      get { return _PathRadiusOffset; }
      set { _PathRadiusOffset = value; }
    }
    private int _PathTaperX;
    [ProtoBuf.ProtoMember(17, IsRequired = true, Name=@"PathTaperX", DataFormat = ProtoBuf.DataFormat.ZigZag)]
    public int PathTaperX
    {
      get { return _PathTaperX; }
      set { _PathTaperX = value; }
    }
    private int _PathTaperY;
    [ProtoBuf.ProtoMember(18, IsRequired = true, Name=@"PathTaperY", DataFormat = ProtoBuf.DataFormat.ZigZag)]
    public int PathTaperY
    {
      get { return _PathTaperY; }
      set { _PathTaperY = value; }
    }
    private uint _PathRevolutions;
    [ProtoBuf.ProtoMember(19, IsRequired = true, Name=@"PathRevolutions", DataFormat = ProtoBuf.DataFormat.TwosComplement)]
    public uint PathRevolutions
    {
      get { return _PathRevolutions; }
      set { _PathRevolutions = value; }
    }
    private int _PathSkew;
    [ProtoBuf.ProtoMember(20, IsRequired = true, Name=@"PathSkew", DataFormat = ProtoBuf.DataFormat.ZigZag)]
    public int PathSkew
    {
      get { return _PathSkew; }
      set { _PathSkew = value; }
    }
    private uint _ProfileBegin;
    [ProtoBuf.ProtoMember(21, IsRequired = true, Name=@"ProfileBegin", DataFormat = ProtoBuf.DataFormat.TwosComplement)]
    public uint ProfileBegin
    {
      get { return _ProfileBegin; }
      set { _ProfileBegin = value; }
    }
    private uint _ProfileEnd;
    [ProtoBuf.ProtoMember(22, IsRequired = true, Name=@"ProfileEnd", DataFormat = ProtoBuf.DataFormat.TwosComplement)]
    public uint ProfileEnd
    {
      get { return _ProfileEnd; }
      set { _ProfileEnd = value; }
    }
    private uint _ProfileHollow;
    [ProtoBuf.ProtoMember(23, IsRequired = true, Name=@"ProfileHollow", DataFormat = ProtoBuf.DataFormat.TwosComplement)]
    public uint ProfileHollow
    {
      get { return _ProfileHollow; }
      set { _ProfileHollow = value; }
    }
    private byte[] _TextureEntry;
    [ProtoBuf.ProtoMember(24, IsRequired = true, Name=@"TextureEntry", DataFormat = ProtoBuf.DataFormat.Default)]
    public byte[] TextureEntry
    {
      get { return _TextureEntry; }
      set { _TextureEntry = value; }
    }
    private byte[] _TextureAnim;
    [ProtoBuf.ProtoMember(25, IsRequired = true, Name=@"TextureAnim", DataFormat = ProtoBuf.DataFormat.Default)]
    public byte[] TextureAnim
    {
      get { return _TextureAnim; }
      set { _TextureAnim = value; }
    }
    private string _NameValue;
    [ProtoBuf.ProtoMember(26, IsRequired = true, Name=@"NameValue", DataFormat = ProtoBuf.DataFormat.Default)]
    public string NameValue
    {
      get { return _NameValue; }
      set { _NameValue = value; }
    }
    private byte[] _Data;
    [ProtoBuf.ProtoMember(27, IsRequired = true, Name=@"Data", DataFormat = ProtoBuf.DataFormat.Default)]
    public byte[] Data
    {
      get { return _Data; }
      set { _Data = value; }
    }
    private string _Text;
    [ProtoBuf.ProtoMember(28, IsRequired = true, Name=@"Text", DataFormat = ProtoBuf.DataFormat.Default)]
    public string Text
    {
      get { return _Text; }
      set { _Text = value; }
    }
    private MsdColor4f _TextColor;
    [ProtoBuf.ProtoMember(29, IsRequired = true, Name=@"TextColor", DataFormat = ProtoBuf.DataFormat.Default)]
    public MsdColor4f TextColor
    {
      get { return _TextColor; }
      set { _TextColor = value; }
    }
    private string _MediaURL;
    [ProtoBuf.ProtoMember(30, IsRequired = true, Name=@"MediaURL", DataFormat = ProtoBuf.DataFormat.Default)]
    public string MediaURL
    {
      get { return _MediaURL; }
      set { _MediaURL = value; }
    }

    private byte[] _PSBlock =null;
    [ProtoBuf.ProtoMember(31, IsRequired = false, Name=@"PSBlock", DataFormat = ProtoBuf.DataFormat.Default)][System.ComponentModel.DefaultValue(null)]
    public byte[] PSBlock
    {
      get { return _PSBlock; }
      set { _PSBlock = value; }
    }

    private byte[] _ExtraParams =null;
    [ProtoBuf.ProtoMember(32, IsRequired = false, Name=@"ExtraParams", DataFormat = ProtoBuf.DataFormat.Default)][System.ComponentModel.DefaultValue(null)]
    public byte[] ExtraParams
    {
      get { return _ExtraParams; }
      set { _ExtraParams = value; }
    }

    private byte[] _SoundId =null;
    [ProtoBuf.ProtoMember(33, IsRequired = false, Name=@"SoundId", DataFormat = ProtoBuf.DataFormat.Default)][System.ComponentModel.DefaultValue(null)]
    public byte[] SoundId
    {
      get { return _SoundId; }
      set { _SoundId = value; }
    }

    private uint _SoundGain =default(uint);
    [ProtoBuf.ProtoMember(34, IsRequired = false, Name=@"SoundGain", DataFormat = ProtoBuf.DataFormat.TwosComplement)][System.ComponentModel.DefaultValue(default(uint))]
    public uint SoundGain
    {
      get { return _SoundGain; }
      set { _SoundGain = value; }
    }

    private float _SoundRadius =default(float);
    [ProtoBuf.ProtoMember(35, IsRequired = false, Name=@"SoundRadius", DataFormat = ProtoBuf.DataFormat.FixedSize)][System.ComponentModel.DefaultValue(default(float))]
    public float SoundRadius
    {
      get { return _SoundRadius; }
      set { _SoundRadius = value; }
    }

    private uint _JointType =default(uint);
    [ProtoBuf.ProtoMember(36, IsRequired = false, Name=@"JointType", DataFormat = ProtoBuf.DataFormat.TwosComplement)][System.ComponentModel.DefaultValue(default(uint))]
    public uint JointType
    {
      get { return _JointType; }
      set { _JointType = value; }
    }

    private MsdVector3f _JointPivot =null;
    [ProtoBuf.ProtoMember(37, IsRequired = false, Name=@"JointPivot", DataFormat = ProtoBuf.DataFormat.Default)][System.ComponentModel.DefaultValue(null)]
    public MsdVector3f JointPivot
    {
      get { return _JointPivot; }
      set { _JointPivot = value; }
    }

    private MsdVector3f _JointAxisOrAnchor =null;
    [ProtoBuf.ProtoMember(38, IsRequired = false, Name=@"JointAxisOrAnchor", DataFormat = ProtoBuf.DataFormat.Default)][System.ComponentModel.DefaultValue(null)]
    public MsdVector3f JointAxisOrAnchor
    {
      get { return _JointAxisOrAnchor; }
      set { _JointAxisOrAnchor = value; }
    }
      private ProtoBuf.IExtension extensionObject;
      ProtoBuf.IExtension ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
  
    [System.Serializable, ProtoBuf.ProtoContract(Name=@"OmModelPrimitiveExt")]
    public partial class OmModelPrimitiveExt : ProtoBuf.IExtensible
    {
      public OmModelPrimitiveExt() {}
      
    private string _ModelUrl;
    [ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"ModelUrl", DataFormat = ProtoBuf.DataFormat.Default)]
    public string ModelUrl
    {
      get { return _ModelUrl; }
      set { _ModelUrl = value; }
    }
    private float _Scale;
    [ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"Scale", DataFormat = ProtoBuf.DataFormat.FixedSize)]
    public float Scale
    {
      get { return _Scale; }
      set { _Scale = value; }
    }
      private ProtoBuf.IExtension extensionObject;
      ProtoBuf.IExtension ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
  
    [System.Serializable, ProtoBuf.ProtoContract(Name=@"OmBitmapTerrainExt")]
    public partial class OmBitmapTerrainExt : ProtoBuf.IExtensible
    {
      public OmBitmapTerrainExt() {}
      
    private uint _Width;
    [ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"Width", DataFormat = ProtoBuf.DataFormat.TwosComplement)]
    public uint Width
    {
      get { return _Width; }
      set { _Width = value; }
    }
    private uint _Height;
    [ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"Height", DataFormat = ProtoBuf.DataFormat.TwosComplement)]
    public uint Height
    {
      get { return _Height; }
      set { _Height = value; }
    }
    private float _WaterLevel;
    [ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"WaterLevel", DataFormat = ProtoBuf.DataFormat.FixedSize)]
    public float WaterLevel
    {
      get { return _WaterLevel; }
      set { _WaterLevel = value; }
    }
    private float _Offset;
    [ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"Offset", DataFormat = ProtoBuf.DataFormat.FixedSize)]
    public float Offset
    {
      get { return _Offset; }
      set { _Offset = value; }
    }
    private float _Scale;
    [ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"Scale", DataFormat = ProtoBuf.DataFormat.FixedSize)]
    public float Scale
    {
      get { return _Scale; }
      set { _Scale = value; }
    }
    private byte[] _HeightMap;
    [ProtoBuf.ProtoMember(6, IsRequired = true, Name=@"HeightMap", DataFormat = ProtoBuf.DataFormat.Default)]
    public byte[] HeightMap
    {
      get { return _HeightMap; }
      set { _HeightMap = value; }
    }
      private ProtoBuf.IExtension extensionObject;
      ProtoBuf.IExtension ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
  
    [System.Serializable, ProtoBuf.ProtoContract(Name=@"OmCamera")]
    public partial class OmCamera : ProtoBuf.IExtensible
    {
      public OmCamera() {}
      
    private MsdVector3f _Location;
    [ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"Location", DataFormat = ProtoBuf.DataFormat.Default)]
    public MsdVector3f Location
    {
      get { return _Location; }
      set { _Location = value; }
    }
    private MsdQuaternion4f _Orientation;
    [ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"Orientation", DataFormat = ProtoBuf.DataFormat.Default)]
    public MsdQuaternion4f Orientation
    {
      get { return _Orientation; }
      set { _Orientation = value; }
    }
      private ProtoBuf.IExtension extensionObject;
      ProtoBuf.IExtension ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
  
    [System.Serializable, ProtoBuf.ProtoContract(Name=@"OmBipedBoneOrientation")]
    public partial class OmBipedBoneOrientation : ProtoBuf.IExtensible
    {
      public OmBipedBoneOrientation() {}
      
    private OmBipedBones _Bone;
    [ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"Bone", DataFormat = ProtoBuf.DataFormat.TwosComplement)]
    public OmBipedBones Bone
    {
      get { return _Bone; }
      set { _Bone = value; }
    }
    private MsdQuaternion4f _Orientation;
    [ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"Orientation", DataFormat = ProtoBuf.DataFormat.Default)]
    public MsdQuaternion4f Orientation
    {
      get { return _Orientation; }
      set { _Orientation = value; }
    }
      private ProtoBuf.IExtension extensionObject;
      ProtoBuf.IExtension ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
  
    [System.Serializable, ProtoBuf.ProtoContract(Name=@"OmCustomBoneOrientation")]
    public partial class OmCustomBoneOrientation : ProtoBuf.IExtensible
    {
      public OmCustomBoneOrientation() {}
      
    private uint _BoneIndex;
    [ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"BoneIndex", DataFormat = ProtoBuf.DataFormat.TwosComplement)]
    public uint BoneIndex
    {
      get { return _BoneIndex; }
      set { _BoneIndex = value; }
    }
    private MsdQuaternion4f _Orientation;
    [ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"Orientation", DataFormat = ProtoBuf.DataFormat.Default)]
    public MsdQuaternion4f Orientation
    {
      get { return _Orientation; }
      set { _Orientation = value; }
    }
      private ProtoBuf.IExtension extensionObject;
      ProtoBuf.IExtension ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
  
    [System.Serializable, ProtoBuf.ProtoContract(Name=@"OmBody")]
    public partial class OmBody : ProtoBuf.IExtensible
    {
      public OmBody() {}
      
    private OmBodyType _BodyType;
    [ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"BodyType", DataFormat = ProtoBuf.DataFormat.TwosComplement)]
    public OmBodyType BodyType
    {
      get { return _BodyType; }
      set { _BodyType = value; }
    }
    private readonly System.Collections.Generic.List<OmBipedBoneOrientation> _BipedBoneOrientations = new System.Collections.Generic.List<OmBipedBoneOrientation>();
    [ProtoBuf.ProtoMember(2, Name=@"BipedBoneOrientations", DataFormat = ProtoBuf.DataFormat.Default)]
    public System.Collections.Generic.List<OmBipedBoneOrientation> BipedBoneOrientations
    {
      get { return _BipedBoneOrientations; }
    }
  
    private readonly System.Collections.Generic.List<OmCustomBoneOrientation> _CustomBoneOrientations = new System.Collections.Generic.List<OmCustomBoneOrientation>();
    [ProtoBuf.ProtoMember(3, Name=@"CustomBoneOrientations", DataFormat = ProtoBuf.DataFormat.Default)]
    public System.Collections.Generic.List<OmCustomBoneOrientation> CustomBoneOrientations
    {
      get { return _CustomBoneOrientations; }
    }
  
      private ProtoBuf.IExtension extensionObject;
      ProtoBuf.IExtension ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
  
    [System.Serializable, ProtoBuf.ProtoContract(Name=@"OmAvatarExt")]
    public partial class OmAvatarExt : ProtoBuf.IExtensible
    {
      public OmAvatarExt() {}
      
    private OmAvatarState _State;
    [ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"State", DataFormat = ProtoBuf.DataFormat.TwosComplement)]
    public OmAvatarState State
    {
      get { return _State; }
      set { _State = value; }
    }

    private OmBody _Body =null;
    [ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"Body", DataFormat = ProtoBuf.DataFormat.Default)][System.ComponentModel.DefaultValue(null)]
    public OmBody Body
    {
      get { return _Body; }
      set { _Body = value; }
    }

    private OmCamera _Camera =null;
    [ProtoBuf.ProtoMember(3, IsRequired = false, Name=@"Camera", DataFormat = ProtoBuf.DataFormat.Default)][System.ComponentModel.DefaultValue(null)]
    public OmCamera Camera
    {
      get { return _Camera; }
      set { _Camera = value; }
    }

    private MsdVector3f _MovementDirection =null;
    [ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"MovementDirection", DataFormat = ProtoBuf.DataFormat.Default)][System.ComponentModel.DefaultValue(null)]
    public MsdVector3f MovementDirection
    {
      get { return _MovementDirection; }
      set { _MovementDirection = value; }
    }

    private MsdVector3f _TargetLocation =null;
    [ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"TargetLocation", DataFormat = ProtoBuf.DataFormat.Default)][System.ComponentModel.DefaultValue(null)]
    public MsdVector3f TargetLocation
    {
      get { return _TargetLocation; }
      set { _TargetLocation = value; }
    }

    private MsdQuaternion4f _TargetOrientation =null;
    [ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"TargetOrientation", DataFormat = ProtoBuf.DataFormat.Default)][System.ComponentModel.DefaultValue(null)]
    public MsdQuaternion4f TargetOrientation
    {
      get { return _TargetOrientation; }
      set { _TargetOrientation = value; }
    }
      private ProtoBuf.IExtension extensionObject;
      ProtoBuf.IExtension ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
  
    public enum OmBipedBones
    {
      Root = 0,
    Pelvis = 1,
    Spine = 2,
    Spine1 = 3,
    Spine2 = 4,
    Spine3 = 5,
    Neck = 6,
    Neck1 = 7,
    Neck2 = 8,
    Neck3 = 9,
    Head = 10,
    LeftClavicle = 11,
    LeftUpperArm = 12,
    LeftForearm = 13,
    LeftHand = 14,
    LeftFinger0 = 15,
    LeftFinger01 = 16,
    LeftFinger02 = 17,
    LeftFinger0Nub = 18,
    LeftFinger1 = 19,
    LeftFinger11 = 20,
    LeftFinger12 = 21,
    LeftFinger1Nub = 22,
    LeftFinger2 = 23,
    LeftFinger21 = 24,
    LeftFinger22 = 25,
    LeftFinger2Nub = 26,
    LeftFinger3 = 27,
    LeftFinger31 = 28,
    LeftFinger32 = 29,
    LeftFinger3Nub = 30,
    LeftFinger4 = 31,
    LeftFinger41 = 32,
    LeftFinger42 = 33,
    LeftFinger4Nub = 34,
    RightClavicle = 35,
    RightUpperArm = 36,
    RightForearm = 37,
    RightHand = 38,
    RightFinger0 = 39,
    RightFinger01 = 40,
    RightFinger02 = 41,
    RightFinger0Nub = 42,
    RightFinger1 = 43,
    RightFinger11 = 44,
    RightFinger12 = 45,
    RightFinger1Nub = 46,
    RightFinger2 = 47,
    RightFinger21 = 48,
    RightFinger22 = 49,
    RightFinger2Nub = 50,
    RightFinger3 = 51,
    RightFinger31 = 52,
    RightFinger32 = 53,
    RightFinger4Nub = 54,
    RightFinger4 = 55,
    RightFinger41 = 56,
    RightFinger42 = 57,
    RightFinger5Nub = 58,
    LeftThigh = 59,
    LeftCalf = 60,
    LeftFoot = 61,
    LeftToe0 = 62,
    LeftToe0Nub = 63,
    RightThigh = 64,
    RightCalf = 65,
    RightFoot = 66,
    RightToe0 = 67,
    RightToe0Nub = 68
    }
  
    public enum OmBodyType
    {
      Custom = 1,
    Biped = 2
    }
  
    public enum OmAvatarState
    {
      None = 0,
    Typing = 4,
    Editing = 16
    }
  
    }
  
    namespace MXP.Common.proto
    {
      
    [System.Serializable, ProtoBuf.ProtoContract(Name=@"MsdVector3f")]
    public partial class MsdVector3f : ProtoBuf.IExtensible
    {
      public MsdVector3f() {}
      
    private float _X;
    [ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"X", DataFormat = ProtoBuf.DataFormat.FixedSize)]
    public float X
    {
      get { return _X; }
      set { _X = value; }
    }
    private float _Y;
    [ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"Y", DataFormat = ProtoBuf.DataFormat.FixedSize)]
    public float Y
    {
      get { return _Y; }
      set { _Y = value; }
    }
    private float _Z;
    [ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"Z", DataFormat = ProtoBuf.DataFormat.FixedSize)]
    public float Z
    {
      get { return _Z; }
      set { _Z = value; }
    }
      private ProtoBuf.IExtension extensionObject;
      ProtoBuf.IExtension ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
  
    [System.Serializable, ProtoBuf.ProtoContract(Name=@"MsdQuaternion4f")]
    public partial class MsdQuaternion4f : ProtoBuf.IExtensible
    {
      public MsdQuaternion4f() {}
      
    private float _X;
    [ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"X", DataFormat = ProtoBuf.DataFormat.FixedSize)]
    public float X
    {
      get { return _X; }
      set { _X = value; }
    }
    private float _Y;
    [ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"Y", DataFormat = ProtoBuf.DataFormat.FixedSize)]
    public float Y
    {
      get { return _Y; }
      set { _Y = value; }
    }
    private float _Z;
    [ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"Z", DataFormat = ProtoBuf.DataFormat.FixedSize)]
    public float Z
    {
      get { return _Z; }
      set { _Z = value; }
    }
    private float _W;
    [ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"W", DataFormat = ProtoBuf.DataFormat.FixedSize)]
    public float W
    {
      get { return _W; }
      set { _W = value; }
    }
      private ProtoBuf.IExtension extensionObject;
      ProtoBuf.IExtension ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
  
    [System.Serializable, ProtoBuf.ProtoContract(Name=@"MsdColor4f")]
    public partial class MsdColor4f : ProtoBuf.IExtensible
    {
      public MsdColor4f() {}
      
    private float _R;
    [ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"R", DataFormat = ProtoBuf.DataFormat.FixedSize)]
    public float R
    {
      get { return _R; }
      set { _R = value; }
    }
    private float _G;
    [ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"G", DataFormat = ProtoBuf.DataFormat.FixedSize)]
    public float G
    {
      get { return _G; }
      set { _G = value; }
    }
    private float _B;
    [ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"B", DataFormat = ProtoBuf.DataFormat.FixedSize)]
    public float B
    {
      get { return _B; }
      set { _B = value; }
    }
    private float _A;
    [ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"A", DataFormat = ProtoBuf.DataFormat.FixedSize)]
    public float A
    {
      get { return _A; }
      set { _A = value; }
    }
      private ProtoBuf.IExtension extensionObject;
      ProtoBuf.IExtension ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
        { return ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
    }
  
    }
  