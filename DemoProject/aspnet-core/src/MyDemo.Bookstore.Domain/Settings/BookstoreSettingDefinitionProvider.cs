using Volo.Abp.Settings;

namespace MyDemo.Bookstore.Settings;

public class BookstoreSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(BookstoreSettings.MySetting1));
    }
}
