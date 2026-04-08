namespace Buildforge.Service.Domain.Misc;

public sealed class FakeStream(int length) : Stream
{
    private readonly Random Random = new();

    public override bool CanRead => throw new NotImplementedException();

    public override bool CanSeek => false;

    public override bool CanWrite => throw new NotImplementedException();

    public override long Length => throw new NotImplementedException();

    public override long Position { get; set; }

    public override void Flush()
    {
        throw new NotImplementedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        throw new NotImplementedException();
    }

    public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        if (Position >= length)
        {
            return 0;
        }

        int remaining = (int)Math.Min(buffer.Length, length - Position);

        await Task.Delay(Random.Next(0, 10), cancellationToken);

        byte[] temp = new byte[remaining];

        Random.NextBytes(temp);

        temp.CopyTo(buffer);

        Position += remaining;

        return remaining;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotImplementedException();
    }

    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotImplementedException();
    }
}