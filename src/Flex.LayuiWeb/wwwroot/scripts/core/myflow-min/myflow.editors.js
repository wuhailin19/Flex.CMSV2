(function($){
var myflow = $.myflow;

$.extend(true, myflow.editors, {
	inputEditor : function(){
		var _props,_k,_div,_src,_r;
		this.init = function(props, k, div, src, r)
		{
		
			_props=props; _k=k; _div=div; _src=src; _r=r;
				 
				$('<input id="input_'+_div+'" style="width:100%;" class="layui-input"/>').val(props[_k].value).change(function(){
					props[_k].value = $(this).val();   
				}).appendTo('#'+_div);
				
				$('#'+_div).data('editor', this);
			}
			this.destroy = function(){ 
			$('#'+_div+' input').each(function(){
			 
				_props[_k].value = $(this).val();
			});
		}
	},
	selectEditor : function(arg){
		var _props,_k,_div,_src,_r;
		this.init = function(props, k, div, src, r){
			_props=props; _k=k; _div=div; _src=src; _r=r;
			
			var stJson = stepArray[_src.getId()];
			
			if('undefined' == typeof(stJson))
			{
				stepArray[_src.getId()] = { "avoidFlag":"0", "orgMode":"0", "stepOrg":"", "stepRole":"", "stepMan":"",  "stepId":"-1", "flowId":"-1" } ;
				
				stJson = stepArray[_src.getId()];
			}	
  		 
  			var currVal = '';
  			
  			if('pavoid' == _div)
			{
				currVal = stJson.avoidFlag;
			}
			else if('porg' == _div)
			{
				currVal = stJson.orgMode;
			}	
		 
			var sle = $('<select id="select_'+_div+'" style="width:100%;" class="lay-selet"/>').val(currVal).change(function(){
				props[_k].value = $(this).val(); 
				 
				 
				if('pavoid' == _div)
				{
					stJson.avoidFlag = $(this).val();
				}
				else if('porg' == _div)
				{
					stJson.orgMode = $(this).val();
				}			
				 
				 
			}).appendTo('#'+_div);
			
			if(typeof arg === 'string'){
				$.ajax({
				   type: "GET",
				   url: arg,
				   success: function(data){
					  var opts = eval(data);
					 if(opts && opts.length){
						for(var idx=0; idx<opts.length; idx++){
							sle.append('<option value="'+opts[idx].value+'">'+opts[idx].name+'</option>');
						}
						sle.val(_props[_k].value);
					 }
				   }
				});
			}else {
				for(var idx=0; idx<arg.length; idx++){
					sle.append('<option value="'+arg[idx].value+'">'+arg[idx].name+'</option>');
				}
				sle.val(currVal);
			}
			
		 	
			$('#'+_div).data('editor', this);
		};
		this.destroy = function(){
			$('#'+_div+' input').each(function(){
				_props[_k].value = $(this).val();
			});
		};
	}
});

})(jQuery);