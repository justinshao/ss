using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;
using Common.Utilities.Helpers;

namespace SmartSystem.WeiXinBase
{
    /// <summary>
    /// 获取菜单时候的完整结构，用于接收微信服务器返回的Json信息
    /// </summary>
    public class FullButtonGroupMenu
    {
        [JsonProperty(PropertyName = "menu")]
        public FullButtonGroup Menu { get; set; }

    }

    public static class MenuExt
    {
        /// <summary>
        /// 根据微信返回的Json数据得到可用的GetMenuResult结果
        /// </summary>
        /// <param name="resultFull"></param>
        /// <returns></returns>
        public static MainButton ToMainButton(this FullButtonGroupMenu resultFull)
        {
            //重新整理按钮信息
            var bg = new MainButton();
            foreach (var rootButton in resultFull.Menu.Button)
            {
                if (rootButton.Name == null)
                {
                    continue;//没有设置一级菜单
                }
                var availableSubButton = rootButton.SubButton.Count(z => !string.IsNullOrEmpty(z.Name));//可用二级菜单按钮数量
                if (availableSubButton == 0)
                {
                    //底部单击按钮
                    if (rootButton.Type == null || (rootButton.Type.Equals("CLICK", StringComparison.OrdinalIgnoreCase)
                        && string.IsNullOrEmpty(rootButton.Key)))
                    {
                        throw new Exception("单击按钮的key不能为空！");
                    }
                    bg.Button.Add(rootButton.ToRealSingleButton());
                }
                else if (availableSubButton < 2)
                {
                    throw new Exception("子菜单至少需要填写2个！");
                }
                else
                {
                    //底部二级菜单
                    var subButton = new SubButton(rootButton.Name);
                    bg.Button.Add(subButton);

                    foreach (var subSubButton in rootButton.SubButton)
                    {
                        if (subSubButton.Name == null)
                        {
                            continue; //没有设置菜单
                        }

                        if (subSubButton.Type.Equals("CLICK", StringComparison.OrdinalIgnoreCase)
                            && string.IsNullOrEmpty(subSubButton.Key))
                        {
                            throw new Exception("单击按钮的key不能为空！");
                        }
                        subButton.SubButtons.Add(subSubButton.ToRealSingleButton());
                    }
                }
            }

            if (bg.Button.Count < 2)
            {
                throw new Exception("一级菜单按钮至少为2个！");
            }
            return bg;
        }
    }

    public class FullButtonGroup
    {
        [JsonProperty(PropertyName = "button")]
        public List<FullButton> Button { get; set; }
    }

    public class FullButton
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "key")]
        public string Key { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }
        [JsonProperty(PropertyName = "sub_button")]
        public List<FullButton> SubButton { get; set; }

        public SingleButton ToRealSingleButton()
        {
            var type = EnumHelper.ParseEnum(Type, WButtonType.Click);
            switch (type)
            {
                case WButtonType.Click:
                    return new SingleClickButton
                    {
                        Name = Name,
                        Key = Key,
                        Type = Type
                    };
                case WButtonType.View:
                    return new SingleViewButton
                    {
                        Name = Name,
                        Url = Url,
                        Type = Type
                    };
                default:
                    throw new Exception("未定义的SingleButton类型");
            }
        }
    }
}
