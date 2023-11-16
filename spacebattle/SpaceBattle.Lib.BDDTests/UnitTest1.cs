using SpaceBattle.Lib;
using TechTalk.SpecFlow;
namespace SpaceBattle.Lib.BDDTests;

[Binding]
    public class РеализоватьПрямолинейноеДвижениеStepDefinitions
    {
        [Given(@"космический корабль имеет угол наклона (.*) град к оси OX")]
        public void GivenКосмическийКорабльИмеетУголНаклонаГрадКОсиOX(int p0)
        {
            
        }

        [Given(@"имеет мгновенную угловую скорость (.*) град")]
        public void GivenИмеетМгновеннуюУгловуюСкоростьГрад(int p0)
        {
            
        }

        [When(@"происходит вращение вокруг собственной оси")]
        public void WhenПроисходитВращениеВокругСобственнойОси()
        {
            
        }

        [Then(@"угол наклона космического корабля к оси OX составляет (.*) град")]
        public void ThenУголНаклонаКосмическогоКорабляКОсиOXСоставляетГрад(int p0)
        {
            
        }

        [Given(@"космический корабль, угол наклона которого невозможно определить")]
        public void GivenКосмическийКорабльУголНаклонаКоторогоНевозможноОпределить()
        {
            
        }

        [Given(@"мгновенную угловую скорость невозможно определить")]
        public void GivenМгновеннуюУгловуюСкоростьНевозможноОпределить()
        {
            
        }

        [Given(@"невозможно изменить угол наклона к оси OX космического корабля")]
        public void GivenНевозможноИзменитьУголНаклонаКОсиOXКосмическогоКорабля()
        {
            
        }
    }