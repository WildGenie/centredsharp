namespace CentrED;


public class LandTile : Tile<LandBlock> {

    public const int Size = 3;
    public static LandTile Empty => new(0, 0, 0, 0);

    public LandTile(ushort id, ushort x, ushort y, sbyte z) : base(null) {
        _id = id;
        _x = x;
        _y = y;
        _z = z;
    }

    public LandTile(BinaryReader reader, LandBlock? owner = null, ushort x = 0, ushort y = 0) : base(owner) {
        _id = reader.ReadUInt16();
        _x = x;
        _y = y;
        _z = reader.ReadSByte();
    }
    

    public new ushort X => _x;
    public new ushort Y => _y;

    public override void Write(BinaryWriter writer) {
        writer.Write(_id);
        writer.Write(_z);
    }
}