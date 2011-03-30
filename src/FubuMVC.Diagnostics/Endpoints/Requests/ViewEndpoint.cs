using System;
using System.Collections.Generic;
using System.Linq;
using FubuCore.Util;
using FubuMVC.Core.Diagnostics;
using FubuMVC.Diagnostics.Models.Requests;

namespace FubuMVC.Diagnostics.Endpoints.Requests
{
	public class ViewEndpoint
	{
		private readonly IRequestHistoryCache _requestCache;

		public ViewEndpoint(IRequestHistoryCache requestCache)
		{
			_requestCache = requestCache;
		}

		public RequestDetailsModel Get(RecordedRequestRequestModel request)
		{
			var report = _requestCache.RecentReports().SingleOrDefault(r => r.Id == request.Id);
			if(report == null)
			{
				throw new UnknownObjectException(request.Id);
			}

			var model = new RequestDetailsModel
			            	{
			            		Report = report,
								Root = Gather(report)
			            	};
			return model;
		}

		public BehaviorDetailsModel Gather(IDebugReport report)
		{
			// TODO -- come back and clean this up. Just getting it up and running for a demo
			BehaviorDetailsModel root = null;
			var behaviors = new Cache<Type, BehaviorDetailsModel>(t =>
			                                                      	{
			                                                      		var model = new BehaviorDetailsModel {BehaviorType = t};
																		if(root == null)
																		{
																			root = model;
																		}

			                                                      		return model;
			                                                      	});
			Type lastBehavior = null;
			report
				.Steps
				.Each(s =>
				      	{
				      		var behaviorType = s.Behavior.BehaviorType;
							if(behaviors.Has(behaviorType) && behaviorType != lastBehavior)
							{
								behaviors[behaviorType].AddAfter(s.Details);
							}
							else
							{
								behaviors[behaviorType].AddBefore(s.Details);
							}

							if(lastBehavior != null && behaviorType != lastBehavior)
							{
								var lastModel = behaviors[lastBehavior];
								if(!lastModel.Equals(root))
								{
									behaviors[behaviorType].Inner = lastModel;
								}
							}

							lastBehavior = behaviorType;
				      	});

			return root;
		}
	}
}