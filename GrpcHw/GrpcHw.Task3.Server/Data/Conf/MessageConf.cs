using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrpcHw.Task3.Server.Data.Conf;

public class MessageConf : IEntityTypeConfiguration<Entities.MessageEntity>
{
    public void Configure(EntityTypeBuilder<Entities.MessageEntity> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Username);
        builder.Property(x => x.Text);
    }
}