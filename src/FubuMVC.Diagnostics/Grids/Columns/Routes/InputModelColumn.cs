﻿using FubuMVC.Core.Registration.Nodes;

namespace FubuMVC.Diagnostics.Grids.Columns.Routes
{
	public class InputModelColumn : GridColumnBase<BehaviorChain>
	{
		public InputModelColumn()
			: base("InputModel")
		{
		}

		public override string ValueFor(BehaviorChain chain)
		{
			return chain.InputType() == null ? string.Empty : chain.InputType().FullName;
		}
	}
}