namespace CentrED;

public delegate void TileHueChanged(StaticTile tile, ushort newZ);
public class StaticTile : Tile<StaticBlock> {
    public TileIdChanged<StaticTile>? OnTileIdChanged;
    public TilePosChanged<StaticTile>? OnTilePosChanged;
    public TileZChanged<StaticTile>? OnTileZChanged;
    public TileHueChanged? OnTileHueChanged;
    
    public const int Size = 7;
    private ushort _hue;
    private byte _localX;
    private byte _localY;
    
    public StaticTile(ushort id, ushort x, ushort y, sbyte z, ushort hue, StaticBlock? owner = null) : base(owner) {
        _id = id;
        _x = x;
        _y = y;
        _z = z;
        _hue = hue;
        
        _localX = (byte)(x % 8);
        _localY = (byte)(y % 8);
    }

    public StaticTile(BinaryReader reader, StaticBlock? owner = null, ushort blockX = 0, ushort blockY = 0) : base(owner) {
        _id = reader.ReadUInt16();
        _localX = reader.ReadByte();
        _localY = reader.ReadByte();
        _z = reader.ReadSByte();
        _hue = reader.ReadUInt16();

        _x = (ushort)(blockX * 8 + _localX);
        _y = (ushort)(blockY * 8 + _localY);
    }
    
    public ushort Hue {
        get => _hue;
        set {
            if (_hue != value) {
                OnTileHueChanged?.Invoke(this, value);
                _hue = value;
                DoChanged();
            }
        }
    }

    public override ushort X { 
        get => _x;
        set {
            if (_x != value) {
                OnTilePosChanged?.Invoke(this, value, _y);
                _x = value;
                _localX = (byte)(_x % 8);
                DoChanged();
            }
        } 
    }

    public override ushort Y { 
        get => _y;
        set {
            if (_y != value) {
                OnTilePosChanged?.Invoke(this, _x, value);
                _y = value;
                _localY = (byte)(_y % 8);
                DoChanged();
            }
        }
    }

    public byte LocalX => _localX;
    public byte LocalY => _localY;

    public void UpdatePriorities(StaticTileData tileData, int solver) {
        PriorityBonus = 0;
        if (!tileData.Flags.HasFlag(TiledataFlag.Background)) PriorityBonus++;

        if (tileData.Height > 0) PriorityBonus = 0;

        Priority = _z + PriorityBonus;
        PrioritySolver = solver;
    }

    public override void Write(BinaryWriter writer) {
        writer.Write(_id);
        writer.Write(_localX);
        writer.Write(_localY);
        writer.Write(_z);
        writer.Write(_hue);
    }
}