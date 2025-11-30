using System.Diagnostics;
using System.Text;

static string Parse(string line)
{
    var dict = new Dictionary<char, string>(){
        {'0', "0000"},
        {'1', "0001"},
        {'2', "0010"},
        {'3', "0011"},
        {'4', "0100"},
        {'5', "0101"},
        {'6', "0110"},
        {'7', "0111"},
        {'8', "1000"},
        {'9', "1001"},
        {'A', "1010"},
        {'B', "1011"},
        {'C', "1100"},
        {'D', "1101"},
        {'E', "1110"},
        {'F', "1111"},
    };
    var binaryStr = string.Join(string.Empty, line.Select(c => dict[c]));

    return binaryStr;
}

static (Packet, int endIdx) GetPacket(string binaryStr, int startIdx)
{
    var version = Convert.ToInt32(binaryStr.Substring(startIdx, 3), 2);
    var typeId = Convert.ToInt32(binaryStr.Substring(startIdx + 3, 3), 2);
    startIdx += 6;
    if (typeId == 4)
    {
        var valueStr = new StringBuilder();
        while (binaryStr[startIdx] != '0')
        {
            startIdx += 1;
            valueStr.Append(binaryStr.Substring(startIdx, 4));
            startIdx += 4;
        }
        startIdx += 1;
        valueStr.Append(binaryStr.Substring(startIdx, 4));
        startIdx += 4;
        return (new Literal(version, typeId, valueStr.ToString()), startIdx);
    }
    else
    {
        var lengthTypeId = (int)char.GetNumericValue(binaryStr, startIdx);
        startIdx += 1;
        if (lengthTypeId == 0)
        {
            var lengthSubPackets = Convert.ToInt32(binaryStr.Substring(startIdx, 15), 2);
            startIdx += 15;
            var subBinaryStr = binaryStr.Substring(startIdx, lengthSubPackets);
            var tmpIdx = 0;
            var packets = new List<Packet>();
            while (tmpIdx < lengthSubPackets)
            {
                var (p, i) = GetPacket(subBinaryStr, tmpIdx);
                tmpIdx = i;
                packets.Add(p);
            }
            return (new Operator(version, typeId, lengthTypeId, packets), startIdx + lengthSubPackets);
        }
        else
        {
            Debug.Assert(lengthTypeId == 1);
            var subPacketCount = Convert.ToInt32(binaryStr.Substring(startIdx, 11), 2);
            startIdx += 11;
            var packets = new List<Packet>(subPacketCount);
            for (int i = 0; i < subPacketCount; i++)
            {
                var (p, endi) = GetPacket(binaryStr, startIdx);
                startIdx = endi;
                packets.Add(p);
            }
            return (new Operator(version, typeId, lengthTypeId, packets), startIdx);
        }
    }
}

static int AddVerNumber(Packet packet)
{
    if (packet is Literal lit)
        return lit.Version;
    else
    {
        var op = (Operator)packet;
        return op.Version + op.Packets.Select(p => AddVerNumber(p)).Sum();
    }
}

static int P1(string line)
{
    var binaryStr = Parse(line);
    var (packet, _) = GetPacket(binaryStr, 0);
    return AddVerNumber(packet);
}

static Int64 P2(string line)
{
    var binaryStr = Parse(line);
    var (packet, _) = GetPacket(binaryStr, 0);
    return packet.Value;
}

Debug.Assert(P1("D2FE28") == 6);
Debug.Assert(P1("8A004A801A8002F478") == 16);
Debug.Assert(P1("620080001611562C8802118E34") == 12);
Debug.Assert(P1("C0015000016115A2E0802F182340") == 23);
Debug.Assert(P1("A0016C880162017C3686B18A3D4780") == 31);

Debug.Assert(P2("C200B40A82") == 3);
Debug.Assert(P2("04005AC33890") == 54);
Debug.Assert(P2("880086C3E88112") == 7);
Debug.Assert(P2("CE00C43D881120") == 9);
Debug.Assert(P2("D8005AC2A8F0") == 1);
Debug.Assert(P2("F600BC2D8F") == 0);
Debug.Assert(P2("9C005AC2F8F0") == 0);
Debug.Assert(P2("9C0141080250320F1802104A08") == 1);

var input = File.ReadLines("input.txt").First();
Debug.Assert(P1(input) == 986);
Debug.Assert(P2(input) == 18234816469452);

abstract class Packet
{
    public Packet(int version, int typeId)
    {
        Version = version;
        TypeId = typeId;
    }

    public int Version { get; }
    public int TypeId { get; }
    public abstract Int64 Value { get; }
}

class Literal : Packet
{
    public Literal(int version, int typeId, string valueStr)
        : base(version, typeId)
    {
        ValueStr = valueStr;
    }

    public string ValueStr { get; }

    public override Int64 Value => Convert.ToInt64(ValueStr, 2);
}

class Operator : Packet
{
    public Operator(int version, int typeId, int lengthTypeId, IList<Packet> packets)
        : base(version, typeId)
    {
        LengthTypeId = lengthTypeId;
        Packets = packets;
    }

    public int LengthTypeId { get; }
    public IList<Packet> Packets { get; }

    public override Int64 Value => TypeId switch
    {
        0 => Packets.Select(p => p.Value).Sum(),
        1 => Packets.Select(p => p.Value).Aggregate(1L, (x, y) => x * y),
        2 => Packets.Select(p => p.Value).Min(),
        3 => Packets.Select(p => p.Value).Max(),
        5 => Packets[0].Value > Packets[1].Value ? 1 : 0,
        6 => Packets[0].Value < Packets[1].Value ? 1 : 0,
        7 => Packets[0].Value == Packets[1].Value ? 1 : 0,
        _ => throw new ArgumentException($"Invalid Type ID: {TypeId}"),
    };
}
