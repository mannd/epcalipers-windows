using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPCalipersWinUI3.Models
{
    public enum ComponentRole
    {
        Left,
        Right,
        Top,
        Bottom,
        Cross,
        Apex
    }
	public class TimeCaliper
	{
		public CaliperComponent LeftBar { get; set; }
		public CaliperComponent RightBar { get; set; }
		public CaliperComponent CrossBar { get; set; }

		public Dictionary<ComponentRole, CaliperComponent> CaliperComponents { get; set; }

		public TimeCaliper()
		{
			LeftBar = new CaliperComponent(ComponentDirection.Vertical);
			RightBar = new CaliperComponent(ComponentDirection.Vertical);
			CrossBar = new CaliperComponent(ComponentDirection.Horizontal);
			CaliperComponents = new()
			{
				{ComponentRole.Left, LeftBar },
				{ComponentRole.Right, RightBar },
				{ComponentRole.Cross, CrossBar }
			};
		}

		public bool IsSelected
		{
			get
			{
				return _isSelected;
			}
			set
			{
				_isSelected = value;
				foreach (var component in CaliperComponents)
				{
					component.Value.IsSelected = value;
				}
			}
		}
		private bool _isSelected;

		public void Move(CaliperComponent component, int distance)
		{
			if (component == null) { return; }
			component.Move(distance);
		}
	}
}
