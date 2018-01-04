
      // Generated from 
    namespace MXP.Common.Proto
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
  