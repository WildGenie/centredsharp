using CentrED.Network;

namespace CentrED.Client; 

public partial class Landscape {
    private void OnBlockPacket(BinaryReader reader, NetState<CentrEDClient> ns) {
        ns.LogDebug("OnBlockPacket");
        var index = new GenericIndex();
        while (reader.PeekChar() != -1) {
            var coords = new BlockCoords(reader);

            var landBlock = new LandBlock(x: coords.X, y: coords.Y, reader: reader);
            foreach (var landTile in landBlock.Tiles) {
                // landTile.OnTileIdChanged = (tile, newTileId) =>
                    // ns.Send(new DrawMapPacket(tile.X, tile.Y, tile.Z, newTileId));
                // landTile.OnTileZChanged = (tile, newZ) =>
                    // ns.Send(new DrawMapPacket(tile.X, tile.Y, newZ, tile.Id));
            }
            var staticsCount = reader.ReadUInt16();
            if(staticsCount > 0 )
                index.Lookup = (int)reader.BaseStream.Position;
            else {
                index.Lookup = -1;
            }
            index.Length = StaticTile.Size * staticsCount;
            
            var staticBlock = new StaticBlock(reader, index, coords.X, coords.Y);
            foreach (var staticTile in staticBlock.Tiles) {
                // staticTile.OnTileIdChanged = (tile, newId) => {
                    // ns.Send(new DeleteStaticPacket(tile));
                    // tile.Id
                        // ns.Send(new InsertStaticPacket());
                // };
                // s
            }
            BlockCache.Add(new Block(landBlock, staticBlock));
        }
    }
}