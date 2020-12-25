namespace Core.Network
{
    public struct Port
    {
        public ushort Number { get; }

        public Port(ushort number) => this.Number = number;

        public static explicit operator Port(ushort value) => new(value);

        public static explicit operator ushort(Port port) => port.Number;
    }
}
