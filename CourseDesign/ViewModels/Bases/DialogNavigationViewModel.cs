using CourseDesign.Extensions;
using Prism.Events;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseDesign.ViewModels.Bases
{
    /// <summary>
    /// 导航所用类的基类，
    /// 对基本的BindableBase进行扩展，使其能支持弹出会话(Dialog)
    /// </summary>
    public class DialogNavigationViewModel : BindableBase, INavigationAware
    {
        private readonly IContainerProvider containerProvider;
        public readonly IEventAggregator aggregator; // 事件聚合器 - 通过这个可以调用各种方法，从而展开等待条等东西……

        public DialogNavigationViewModel(IContainerProvider containerProvider)
        {
            this.containerProvider = containerProvider;
            aggregator = containerProvider.Resolve<IEventAggregator>();
        }

        /// <summary>
        /// 是否重用以前的窗口而非重新加载
        /// </summary>
        /// <param name="navigationContext">窗口上下文？……</param>
        /// <returns>恒不重用</returns>
        public virtual bool IsNavigationTarget(NavigationContext navigationContext) { return true; }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext) { }

        public virtual void OnNavigatedTo(NavigationContext navigationContext) { }

        /// <summary>
        /// 展开等待窗口
        /// </summary>
        /// <param name="IsOpen">是否展开</param>
        public void ShowLoadingDialog(bool IsOpen)
        {
            aggregator.ShowLoadingDialog(new Common.Events.LoadingModel()
            {
                IsOpen = IsOpen
            });
        }

        /// <summary>
        /// 发布消息，展开底部浮动提示消息弹窗
        /// </summary>
        /// <param name="message">发送的消息</param>
        /// <param name="filterName">过滤器（发送到哪去，主界面的为Main；登陆界面的为Login）</param>
        public void ShowMessageDialog(string msg, string filterName)
        {
            aggregator.ShowMessageDialog(msg, filterName);
        }
    }
}
