/*
 * $Id: om-core.js,v 1.27 2012/06/19 08:40:21 licongping Exp $
 * operamasks-ui om-core @VERSION
 *
 * Copyright 2011, AUTHORS.txt (http://ui.operamasks.org/about)
 * Dual licensed under the MIT or LGPL Version 2 licenses.
 * http://ui.operamasks.org/license
 *
 * http://ui.operamasks.org/docs/
 */
(function( $, undefined ) {
// prevent duplicate loading
// this is only a problem because we proxy existing functions
// and we don't want to double proxy them
$.om = $.om || {};
if ( $.om.version ) {
	return;
}

$.extend( $.om, {
	version: "2.0",
	keyCode: {
	    TAB: 9,
	    ENTER: 13,
	    ESCAPE: 27,
	    SPACE: 32,
		LEFT: 37,
		UP: 38,
		RIGHT: 39,
		DOWN: 40
	},
	lang : {
		// 获取属性的国际化字符串，如果组件的options中已经设置这个值就直接使用，否则从$.om.lang[comp]中获取
		_get : function(options, comp, attr){
			return options[attr] ? options[attr] : $.om.lang[comp][attr]; 
		}
	}
});
// plugins
$.fn.extend({
	propAttr: $.fn.prop || $.fn.attr,
	_oldFocus: $.fn.focus,//为避免与jQuery ui冲突导致死循环，这里不要取名为'_focus'
	//设置元素焦点（delay：延迟时间）
	focus: function( delay, fn ) {
		return typeof delay === "number" ?
			this.each(function() {
				var elem = this;
				setTimeout(function() {
					$( elem ).focus();
					if ( fn ) {
						fn.call( elem );
					}
				}, delay );
			}) :
			this._oldFocus.apply( this, arguments );
	},
	//获取设置滚动属性的 父元素
	scrollParent: function() {
		var scrollParent;
		if (($.browser.msie && (/(static|relative)/).test(this.css('position'))) || (/absolute/).test(this.css('position'))) {
			scrollParent = this.parents().filter(function() {
				return (/(relative|absolute|fixed)/).test($.curCSS(this,'position',1)) && (/(auto|scroll)/).test($.curCSS(this,'overflow',1)+$.curCSS(this,'overflow-y',1)+$.curCSS(this,'overflow-x',1));
			}).eq(0);
		} else {
			scrollParent = this.parents().filter(function() {
				return (/(auto|scroll)/).test($.curCSS(this,'overflow',1)+$.curCSS(this,'overflow-y',1)+$.curCSS(this,'overflow-x',1));
			}).eq(0);
		}
		return (/fixed/).test(this.css('position')) || !scrollParent.length ? $(document) : scrollParent;
	},
	//设置或获取元素的垂直坐标
	zIndex: function( zIndex ) {
		if ( zIndex !== undefined ) {
			return this.css( "zIndex", zIndex );
		}
		if ( this.length ) {
			var elem = $( this[ 0 ] ), position, value;
			while ( elem.length && elem[ 0 ] !== document ) {
				// Ignore z-index if position is set to a value where z-index is ignored by the browser
				// This makes behavior of this function consistent across browsers
				// WebKit always returns auto if the element is positioned
				position = elem.css( "position" );
				if ( position === "absolute" || position === "relative" || position === "fixed" ) {
					// IE returns 0 when zIndex is not specified
					// other browsers return a string
					// we ignore the case of nested elements with an explicit value of 0
					// <div style="z-index: -10;"><div style="z-index: 0;"></div></div>
					value = parseInt( elem.css( "zIndex" ), 10 );
					if ( !isNaN( value ) && value !== 0 ) {
						return value;
					}
				}
				elem = elem.parent();
			}
		}
		return 0;
	},
	//设置元素不支持被选择
	disableSelection: function() {
		return this.bind( ( $.support.selectstart ? "selectstart" : "mousedown" ) +
			".om-disableSelection", function( event ) {
				event.preventDefault();
			});
	},
	//设置元素支持被选择
	enableSelection: function() {
		return this.unbind( ".om-disableSelection" );
	}
});
// 扩展innerWidth、innerHeight、outerWidth和outerHeight方法，如果不传参则获取值，如果传参则设置计算后的宽高。
$.each( [ "Width", "Height" ], function( i, name ) {
	var side = name === "Width" ? [ "Left", "Right" ] : [ "Top", "Bottom" ],
		type = name.toLowerCase(),
		orig = {
			innerWidth: $.fn.innerWidth,
			innerHeight: $.fn.innerHeight,
			outerWidth: $.fn.outerWidth,
			outerHeight: $.fn.outerHeight
		};

	function reduce( elem, size, border, margin ) {
		$.each( side, function() {
			size -= parseFloat( $.curCSS( elem, "padding" + this, true) ) || 0;
			if ( border ) {
				size -= parseFloat( $.curCSS( elem, "border" + this + "Width", true) ) || 0;
			}
			if ( margin ) {
				size -= parseFloat( $.curCSS( elem, "margin" + this, true) ) || 0;
			}
		});
		return size;
	}

	$.fn[ "inner" + name ] = function( size ) {
		if ( size === undefined ) {
			// 返回innerWidth/innerHeight
			return orig[ "inner" + name ].call( this );
		}
		return this.each(function() {
			// 设置宽度/高度 = (size - padding)
			$( this ).css( type, reduce( this, size ) + "px" );
		});
	};

	$.fn[ "outer" + name] = function( size, margin ) {
		if ( typeof size !== "number" ) {
			// 返回outerWidth/outerHeight
			return orig[ "outer" + name ].call( this, size );
		}
		return this.each(function() {
			// 设置宽度/高度 = (size - padding - border - margin)
			$( this).css( type, reduce( this, size, true, margin ) + "px" );
		});
	};
});
// selectors
function focusable( element, isTabIndexNotNaN ) {
	var nodeName = element.nodeName.toLowerCase();
	if ( "area" === nodeName ) {
		var map = element.parentNode,
			mapName = map.name,
			img;
		if ( !element.href || !mapName || map.nodeName.toLowerCase() !== "map" ) {
			return false;
		}
		img = $( "img[usemap=#" + mapName + "]" )[0];
		return !!img && visible( img );
	}
	return ( /input|select|textarea|button|object/.test( nodeName )
		? !element.disabled
		: "a" == nodeName
			? element.href || isTabIndexNotNaN
			: isTabIndexNotNaN)
		// the element and all of its ancestors must be visible
		&& visible( element );
}
function visible( element ) {
	return !$( element ).parents().andSelf().filter(function() {
		return $.curCSS( this, "visibility" ) === "hidden" ||
			$.expr.filters.hidden( this );
	}).length;
}
$.extend( $.expr[ ":" ], {
	data: function( elem, i, match ) {
		return !!$.data( elem, match[ 3 ] );
	},
	focusable: function( element ) {
		return focusable( element, !isNaN( $.attr( element, "tabindex" ) ) );
	},
	tabbable: function( element ) {
		var tabIndex = $.attr( element, "tabindex" ),
			isTabIndexNaN = isNaN( tabIndex );
		return ( isTabIndexNaN || tabIndex >= 0 ) && focusable( element, !isTabIndexNaN );
	}
});
// support
$(function() {
	var body = document.body,
		div = body.appendChild( div = document.createElement( "div" ) );
	$.extend( div.style, {
		minHeight: "100px",
		height: "auto",
		padding: 0,
		borderWidth: 0
	});
	// 判断当前浏览器环境是否支持minHeight属性
	$.support.minHeight = div.offsetHeight === 100;
	$.support.selectstart = "onselectstart" in div;
	// set display to none to avoid a layout bug in IE
	// http://dev.jquery.com/ticket/4014
	body.removeChild( div ).style.display = "none";
});

// deprecated
$.extend( $.om, {
	// $.om.plugin is deprecated.  Use the proxy pattern instead.
	plugin: {
		add: function( module, option, set ) {
			var proto = $.om[module].prototype;
			for ( var i in set ) {
				proto.plugins[ i ] = proto.plugins[ i ] || [];
				proto.plugins[ i ].push( [ option, set[ i ] ] );
			}
		},
		call: function( instance, name, args ) {
			var set = instance.plugins[ name ];
			if ( !set || !instance.element[ 0 ].parentNode ) {
				return;
			}
			for ( var i = 0; i < set.length; i++ ) {
				if ( instance.options[ set[ i ][ 0 ] ] ) {
					set[ i ][ 1 ].apply( instance.element, args );
				}
			}
		}
	}
});

})( jQuery );


(function( $, undefined ) {
// jQuery 1.4+
if ( $.cleanData ) {
	var _cleanData = $.cleanData;
	$.cleanData = function( elems ) {
		for ( var i = 0, elem; (elem = elems[i]) != null; i++ ) { 
			$( elem ).triggerHandler( "om-remove" );
		}
		_cleanData( elems );
	};
}

$.omWidget = function( name, base, prototype ) {
	var namespace = name.split( "." )[ 0 ],
		fullName;
	name = name.split( "." )[ 1 ];
	fullName = namespace + "-" + name;
	// 例如参数name='om.tabs'，变成namespace='om',name='tabs',fullName='om-tabs' 
	// base默认为Widget类，组件默认会继承base类的所有方法  
	if ( !prototype ) {
		prototype = base;
		base = $.OMWidget;
	}
	// create selector for plugin
	$.expr[ ":" ][ fullName ] = function( elem ) {
		return !!$.data( elem, name );
	};
	// 创建命名空间$.om.tabs  
	$[ namespace ] = $[ namespace ] || {};
	// 组件的构造函数
	$[ namespace ][ name ] = function( options, element ) {
		// allow instantiation without initializing for simple inheritance
		if ( arguments.length ) {
			this._createWidget( options, element );
		}
	};
	// 初始化父类，一般调用了$.Widget  
	var basePrototype = new base();
	// we need to make the options hash a property directly on the new instance
	// otherwise we'll modify the options hash on the prototype that we're
	// inheriting from
//		$.each( basePrototype, function( key, val ) {
//			if ( $.isPlainObject(val) ) {
//				basePrototype[ key ] = $.extend( {}, val );
//			}
//		});
	basePrototype.options = $.extend( true, {}, basePrototype.options );
	// 给om.tabs继承父类的所有原型方法和参数  
	$[ namespace ][ name ].prototype = $.extend( true, basePrototype, {
		namespace: namespace,
		widgetName: name,
		// 组件的事件名前缀，调用_trigger的时候会默认给trigger的事件加上前缀  
        // 例如_trigger('create')实际会触发'tabscreate'事件  
		widgetEventPrefix: $[ namespace ][ name ].prototype.widgetEventPrefix || name,
		widgetBaseClass: fullName
	}, prototype );
	// 把tabs方法挂到jquery对象上，也就是$('#tab1').tabs();  
	$.omWidget.bridge( name, $[ namespace ][ name ] );
};

$.omWidget.bridge = function( name, object ) {
	$.fn[ name ] = function( options ) {
		// 如果tabs方法第一个参数是string类型，则认为是调用组件的方法，否则调用options方法  
		var isMethodCall = typeof options === "string",
			args = Array.prototype.slice.call( arguments, 1 ),
			returnValue = this;
		// allow multiple hashes to be passed on init
		options = !isMethodCall && args.length ?
			$.extend.apply( null, [ true, options ].concat(args) ) :
			options;
		// '_'开头的方法被认为是内部方法，不会被执行，如$('#tab1').tabs('_init')  
		if ( isMethodCall && options.charAt( 0 ) === "_" ) {
			return returnValue;
		}
		if ( isMethodCall ) {
			this.each(function() {
				// 执行组件方法  
				var instance = $.data( this, name );
				if (options == 'options') {
				    returnValue = instance && instance.options;
				    return false;
                } else {
    				var	methodValue = instance && $.isFunction( instance[options] ) ?
    						instance[ options ].apply( instance, args ) : instance;
    				if ( methodValue !== instance && methodValue !== undefined ) {
    					returnValue = methodValue;
    					return false;
    				}
                }
			});
		} else {
			// 调用组件的options方法  
			this.each(function() {
				var instance = $.data( this, name );
				if ( instance ) {
					// 设置options后再次调用_init方法，第一次调用是在_createWidget方法里面。这个方法需要开发者去实现。  
                    // 主要是当改变组件中某些参数后可能需要对组件进行重画  
                    instance._setOptions( options || {} );
				    $.extend(instance.options, options);
				    $(instance.beforeInitListeners).each(function(){
				        this.call(instance);
				    });
					instance._init();
					$(instance.initListeners).each(function(){
				        this.call(instance);
				    });
				} else {
					// 没有实例的话，在这里调用组件类的构造函数，并把构造后的示例保存在dom的data里面。注意这里的this是dom，object是模块类 
					$.data( this, name, new object( options, this ) );
				}
			});
		}

		return returnValue;
	};
};
$.omWidget.addCreateListener = function(name,fn){
    var temp=name.split( "." );
    $[ temp[0] ][ temp[1] ].prototype.createListeners.push(fn);
};
$.omWidget.addInitListener = function(name,fn){
    var temp=name.split( "." );
    $[ temp[0] ][ temp[1] ].prototype.initListeners.push(fn);
};
$.omWidget.addBeforeInitListener = function(name,fn){
    var temp=name.split( "." );
    $[ temp[0] ][ temp[1] ].prototype.beforeInitListeners.push(fn);
};
$.OMWidget = function( options, element ) {
    this.createListeners=[];
    this.initListeners=[];
    this.beforeInitListeners=[];
	// allow instantiation without initializing for simple inheritance
	if ( arguments.length ) {
		this._createWidget( options, element );
	}
};
$.OMWidget.prototype = {
	widgetName: "widget",
	widgetEventPrefix: "",
	options: {
		disabled: false
	},
	_createWidget: function( options, element ) {
		// $.widget.bridge stores the plugin instance, but we do it anyway
		// so that it's stored even before the _create function runs
		$.data( element, this.widgetName, this );
		this.element = $( element );
		this.options = $.extend( true, {},
			this.options,
			this._getCreateOptions(),
			options );
		var self = this;
		//注意，不要少了前边的 "om-"，不然会与jquery-ui冲突
		this.element.bind( "om-remove._" + this.widgetName, function() {
			self.destroy();
		});
		// 开发者实现  
		this._create();
		$(this.createListeners).each(function(){
	        this.call(self);
	    });
		// 如果绑定了初始化的回调函数，会在这里触发。注意绑定的事件名是需要加上前缀的，如$('#tab1').bind('tabscreate',function(){});  
		this._trigger( "create" );
		// 开发者实现 
		$(this.beforeInitListeners).each(function(){
	        this.call(self);
	    });
		this._init();
		$(this.initListeners).each(function(){
	        this.call(self);
	    });
	},
	_getCreateOptions: function() {
		return $.metadata && $.metadata.get( this.element[0] )[ this.widgetName ];
	},
	_create: function() {},
	_init: function() {},
	destroy: function() {
		this.element
			.unbind( "." + this.widgetName )
			.removeData( this.widgetName );
		this.widget()
			.unbind( "." + this.widgetName );
	},
	widget: function() {
		return this.element;
	},
	option: function( key, value ) {
        var options = key;
        if ( arguments.length === 0 ) {
            // don't return a reference to the internal hash
            return $.extend( {}, this.options );
        }
        if  (typeof key === "string" ) {
            if ( value === undefined ) {
                return this.options[ key ]; // 获取值
            }
            options = {};
            options[ key ] = value;
        }
        this._setOptions( options ); // 设置值
        return this;
    },
	_setOptions: function( options ) {
		var self = this;
		$.each( options, function( key, value ) {
			self._setOption( key, value );
		});
		return this;
	},
	_setOption: function( key, value ) {
		this.options[ key ] = value;
		return this;
	},
	
	// $.widget中优化过的trigger方法。type是回调事件的名称，如"onRowClick"，event是触发回调的事件（通常没有这个事件的时候传null）
	// 这个方法只声明了两个参数，如有其他参数可以直接写在event参数后面
	_trigger: function( type, event ) {
		// 获取初始化配置config中的回调方法
		var callback = this.options[ type ];
		// 封装js标准event对象为jquery的Event对象
		event = $.Event( event );
		event.type = type;
		// copy original event properties over to the new event
		// this would happen if we could call $.event.fix instead of $.Event
		// but we don't have a way to force an event to be fixed multiple times
		if ( event.originalEvent ) {
			for ( var i = $.event.props.length, prop; i; ) {
				prop = $.event.props[ --i ];
				event[ prop ] = event.originalEvent[ prop ];
			}
		}
		// 构造传给回调函数的参数，event放置在最后
		var newArgs = [],
			argLength = arguments.length;
		for(var i = 2; i < argLength; i++){
			newArgs[i-2] = arguments[i];
		}
		if( argLength > 1){
			newArgs[argLength-2] = arguments[1];
		}
		return !( $.isFunction(callback) &&
			callback.apply( this.element, newArgs ) === false ||
			event.isDefaultPrevented() );
	}
};
})( jQuery );/*
 * $Id: om-fileupload.js,v 1.29 2012/05/08 06:49:11 linxiaomin Exp $
 * operamasks-ui omFileUpload @VERSION
 *
 * Copyright 2011, AUTHORS.txt (http://ui.operamasks.org/about)
 * Dual licensed under the MIT or LGPL Version 2 licenses.
 * http://ui.operamasks.org/license
 *
 * http://ui.operamasks.org/docs/
 *
 * Depends:
 */
    /** 
     * @name omFileUpload
     * @class 文件上传.<br/>
     * <b>特点：</b><br/>
     * <ol>
     * 		<li>支持限制上传文件的大小和类型</li>
     * 		<li>支持批量上传文件</li>
     * 		<li>内置进度条展示文件上传进度</li>
     * 		<li>支持选中，上传成功，上传失败等多种回调事件</li>
     * 		<li>可自定义上传按钮的背景图片和文字</li>
     * </ol>
     * <b>示例：</b><br/>
     * <pre>
     * <b>//注意：这里的swf属性指定了处理上传的swf文件位置，必须设置。这个位置是相对于HTML页面路径的。</b>
     * &lt;script type="text/javascript" &gt;
     * $(document).ready(function() {
     *     $('#file_upload').omFileUpload({
     *         action : '/operamasks-ui/omFileUpload.do',
     *         onComplete : function(ID,fileObj,response,data,event){
     *             alert('文件'+fileObj.name+'上传完毕');
     *         }
     *     });
     * });
     * &lt;/script&gt;
     * 
     * <b>//注意：input的id属性必须设置</b>
     * &lt;input id="file_upload" name="file_upload" type="file" /&gt;
	 * </pre>
     * @constructor
     * @description 构造函数. 
     * @param p 标准config对象：{}
     */
var swfobject=function(){var D="undefined",r="object",S="Shockwave Flash",W="ShockwaveFlash.ShockwaveFlash",q="application/x-shockwave-flash",R="SWFObjectExprInst",x="onreadystatechange",O=window,j=document,t=navigator,T=false,U=[h],o=[],N=[],I=[],l,Q,E,B,J=false,a=false,n,G,m=true,M=function(){var aa=typeof j.getElementById!=D&&typeof j.getElementsByTagName!=D&&typeof j.createElement!=D,ah=t.userAgent.toLowerCase(),Y=t.platform.toLowerCase(),ae=Y?/win/.test(Y):/win/.test(ah),ac=Y?/mac/.test(Y):/mac/.test(ah),af=/webkit/.test(ah)?parseFloat(ah.replace(/^.*webkit\/(\d+(\.\d+)?).*$/,"$1")):false,X=!+"\v1",ag=[0,0,0],ab=null;if(typeof t.plugins!=D&&typeof t.plugins[S]==r){ab=t.plugins[S].description;if(ab&&!(typeof t.mimeTypes!=D&&t.mimeTypes[q]&&!t.mimeTypes[q].enabledPlugin)){T=true;X=false;ab=ab.replace(/^.*\s+(\S+\s+\S+$)/,"$1");ag[0]=parseInt(ab.replace(/^(.*)\..*$/,"$1"),10);ag[1]=parseInt(ab.replace(/^.*\.(.*)\s.*$/,"$1"),10);ag[2]=/[a-zA-Z]/.test(ab)?parseInt(ab.replace(/^.*[a-zA-Z]+(.*)$/,"$1"),10):0}}else{if(typeof O.ActiveXObject!=D){try{var ad=new ActiveXObject(W);if(ad){ab=ad.GetVariable("$version");if(ab){X=true;ab=ab.split(" ")[1].split(",");ag=[parseInt(ab[0],10),parseInt(ab[1],10),parseInt(ab[2],10)]}}}catch(Z){}}}return{w3:aa,pv:ag,wk:af,ie:X,win:ae,mac:ac}}(),k=function(){if(!M.w3){return}if((typeof j.readyState!=D&&j.readyState=="complete")||(typeof j.readyState==D&&(j.getElementsByTagName("body")[0]||j.body))){f()}if(!J){if(typeof j.addEventListener!=D){j.addEventListener("DOMContentLoaded",f,false)}if(M.ie&&M.win){j.attachEvent(x,function(){if(j.readyState=="complete"){j.detachEvent(x,arguments.callee);f()}});if(O==top){(function(){if(J){return}try{j.documentElement.doScroll("left")}catch(X){setTimeout(arguments.callee,0);return}f()})()}}if(M.wk){(function(){if(J){return}if(!/loaded|complete/.test(j.readyState)){setTimeout(arguments.callee,0);return}f()})()}s(f)}}();function f(){if(J){return}try{var Z=j.getElementsByTagName("body")[0].appendChild(C("span"));Z.parentNode.removeChild(Z)}catch(aa){return}J=true;var X=U.length;for(var Y=0;Y<X;Y++){U[Y]()}}function K(X){if(J){X()}else{U[U.length]=X}}function s(Y){if(typeof O.addEventListener!=D){O.addEventListener("load",Y,false)}else{if(typeof j.addEventListener!=D){j.addEventListener("load",Y,false)}else{if(typeof O.attachEvent!=D){i(O,"onload",Y)}else{if(typeof O.onload=="function"){var X=O.onload;O.onload=function(){X();Y()}}else{O.onload=Y}}}}}function h(){if(T){V()}else{H()}}function V(){var X=j.getElementsByTagName("body")[0];var aa=C(r);aa.setAttribute("type",q);var Z=X.appendChild(aa);if(Z){var Y=0;(function(){if(typeof Z.GetVariable!=D){var ab=Z.GetVariable("$version");if(ab){ab=ab.split(" ")[1].split(",");M.pv=[parseInt(ab[0],10),parseInt(ab[1],10),parseInt(ab[2],10)]}}else{if(Y<10){Y++;setTimeout(arguments.callee,10);return}}X.removeChild(aa);Z=null;H()})()}else{H()}}function H(){var ag=o.length;if(ag>0){for(var af=0;af<ag;af++){var Y=o[af].id;var ab=o[af].callbackFn;var aa={success:false,id:Y};if(M.pv[0]>0){var ae=c(Y);if(ae){if(F(o[af].swfVersion)&&!(M.wk&&M.wk<312)){w(Y,true);if(ab){aa.success=true;aa.ref=z(Y);ab(aa)}}else{if(o[af].expressInstall&&A()){var ai={};ai.data=o[af].expressInstall;ai.width=ae.getAttribute("width")||"0";ai.height=ae.getAttribute("height")||"0";if(ae.getAttribute("class")){ai.styleclass=ae.getAttribute("class")}if(ae.getAttribute("align")){ai.align=ae.getAttribute("align")}var ah={};var X=ae.getElementsByTagName("param");var ac=X.length;for(var ad=0;ad<ac;ad++){if(X[ad].getAttribute("name").toLowerCase()!="movie"){ah[X[ad].getAttribute("name")]=X[ad].getAttribute("value")}}P(ai,ah,Y,ab)}else{p(ae);if(ab){ab(aa)}}}}}else{w(Y,true);if(ab){var Z=z(Y);if(Z&&typeof Z.SetVariable!=D){aa.success=true;aa.ref=Z}ab(aa)}}}}}function z(aa){var X=null;var Y=c(aa);if(Y&&Y.nodeName=="OBJECT"){if(typeof Y.SetVariable!=D){X=Y}else{var Z=Y.getElementsByTagName(r)[0];if(Z){X=Z}}}return X}function A(){return !a&&F("6.0.65")&&(M.win||M.mac)&&!(M.wk&&M.wk<312)}function P(aa,ab,X,Z){a=true;E=Z||null;B={success:false,id:X};var ae=c(X);if(ae){if(ae.nodeName=="OBJECT"){l=g(ae);Q=null}else{l=ae;Q=X}aa.id=R;if(typeof aa.width==D||(!/%$/.test(aa.width)&&parseInt(aa.width,10)<310)){aa.width="310"}if(typeof aa.height==D||(!/%$/.test(aa.height)&&parseInt(aa.height,10)<137)){aa.height="137"}j.title=j.title.slice(0,47)+" - Flash Player Installation";var ad=M.ie&&M.win?"ActiveX":"PlugIn",ac="MMredirectURL="+O.location.toString().replace(/&/g,"%26")+"&MMplayerType="+ad+"&MMdoctitle="+j.title;if(typeof ab.flashvars!=D){ab.flashvars+="&"+ac}else{ab.flashvars=ac}if(M.ie&&M.win&&ae.readyState!=4){var Y=C("div");X+="SWFObjectNew";Y.setAttribute("id",X);ae.parentNode.insertBefore(Y,ae);ae.style.display="none";(function(){if(ae.readyState==4){ae.parentNode.removeChild(ae)}else{setTimeout(arguments.callee,10)}})()}u(aa,ab,X)}}function p(Y){if(M.ie&&M.win&&Y.readyState!=4){var X=C("div");Y.parentNode.insertBefore(X,Y);X.parentNode.replaceChild(g(Y),X);Y.style.display="none";(function(){if(Y.readyState==4){Y.parentNode.removeChild(Y)}else{setTimeout(arguments.callee,10)}})()}else{Y.parentNode.replaceChild(g(Y),Y)}}function g(ab){var aa=C("div");if(M.win&&M.ie){aa.innerHTML=ab.innerHTML}else{var Y=ab.getElementsByTagName(r)[0];if(Y){var ad=Y.childNodes;if(ad){var X=ad.length;for(var Z=0;Z<X;Z++){if(!(ad[Z].nodeType==1&&ad[Z].nodeName=="PARAM")&&!(ad[Z].nodeType==8)){aa.appendChild(ad[Z].cloneNode(true))}}}}}return aa}function u(ai,ag,Y){var X,aa=c(Y);if(M.wk&&M.wk<312){return X}if(aa){if(typeof ai.id==D){ai.id=Y}if(M.ie&&M.win){var ah="";for(var ae in ai){if(ai[ae]!=Object.prototype[ae]){if(ae.toLowerCase()=="data"){ag.movie=ai[ae]}else{if(ae.toLowerCase()=="styleclass"){ah+=' class="'+ai[ae]+'"'}else{if(ae.toLowerCase()!="classid"){ah+=" "+ae+'="'+ai[ae]+'"'}}}}}var af="";for(var ad in ag){if(ag[ad]!=Object.prototype[ad]){af+='<param name="'+ad+'" value="'+ag[ad]+'" />'}}aa.outerHTML='<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"'+ah+">"+af+"</object>";N[N.length]=ai.id;X=c(ai.id)}else{var Z=C(r);Z.setAttribute("type",q);for(var ac in ai){if(ai[ac]!=Object.prototype[ac]){if(ac.toLowerCase()=="styleclass"){Z.setAttribute("class",ai[ac])}else{if(ac.toLowerCase()!="classid"){Z.setAttribute(ac,ai[ac])}}}}for(var ab in ag){if(ag[ab]!=Object.prototype[ab]&&ab.toLowerCase()!="movie"){e(Z,ab,ag[ab])}}aa.parentNode.replaceChild(Z,aa);X=Z}}return X}function e(Z,X,Y){var aa=C("param");aa.setAttribute("name",X);aa.setAttribute("value",Y);Z.appendChild(aa)}function y(Y){var X=c(Y);if(X&&X.nodeName=="OBJECT"){if(M.ie&&M.win){X.style.display="none";(function(){if(X.readyState==4){b(Y)}else{setTimeout(arguments.callee,10)}})()}else{X.parentNode.removeChild(X)}}}function b(Z){var Y=c(Z);if(Y){for(var X in Y){if(typeof Y[X]=="function"){Y[X]=null}}Y.parentNode.removeChild(Y)}}function c(Z){var X=null;try{X=j.getElementById(Z)}catch(Y){}return X}function C(X){return j.createElement(X)}function i(Z,X,Y){Z.attachEvent(X,Y);I[I.length]=[Z,X,Y]}function F(Z){var Y=M.pv,X=Z.split(".");X[0]=parseInt(X[0],10);X[1]=parseInt(X[1],10)||0;X[2]=parseInt(X[2],10)||0;return(Y[0]>X[0]||(Y[0]==X[0]&&Y[1]>X[1])||(Y[0]==X[0]&&Y[1]==X[1]&&Y[2]>=X[2]))?true:false}function v(ac,Y,ad,ab){if(M.ie&&M.mac){return}var aa=j.getElementsByTagName("head")[0];if(!aa){return}var X=(ad&&typeof ad=="string")?ad:"screen";if(ab){n=null;G=null}if(!n||G!=X){var Z=C("style");Z.setAttribute("type","text/css");Z.setAttribute("media",X);n=aa.appendChild(Z);if(M.ie&&M.win&&typeof j.styleSheets!=D&&j.styleSheets.length>0){n=j.styleSheets[j.styleSheets.length-1]}G=X}if(M.ie&&M.win){if(n&&typeof n.addRule==r){n.addRule(ac,Y)}}else{if(n&&typeof j.createTextNode!=D){n.appendChild(j.createTextNode(ac+" {"+Y+"}"))}}}function w(Z,X){if(!m){return}var Y=X?"visible":"hidden";if(J&&c(Z)){c(Z).style.visibility=Y}else{v("#"+Z,"visibility:"+Y)}}function L(Y){var Z=/[\\\"<>\.;]/;var X=Z.exec(Y)!=null;return X&&typeof encodeURIComponent!=D?encodeURIComponent(Y):Y}var d=function(){if(M.ie&&M.win){window.attachEvent("onunload",function(){var ac=I.length;for(var ab=0;ab<ac;ab++){I[ab][0].detachEvent(I[ab][1],I[ab][2])}var Z=N.length;for(var aa=0;aa<Z;aa++){y(N[aa])}for(var Y in M){M[Y]=null}M=null;for(var X in swfobject){swfobject[X]=null}swfobject=null})}}();return{registerObject:function(ab,X,aa,Z){if(M.w3&&ab&&X){var Y={};Y.id=ab;Y.swfVersion=X;Y.expressInstall=aa;Y.callbackFn=Z;o[o.length]=Y;w(ab,false)}else{if(Z){Z({success:false,id:ab})}}},getObjectById:function(X){if(M.w3){return z(X)}},embedSWF:function(ab,ah,ae,ag,Y,aa,Z,ad,af,ac){var X={success:false,id:ah};if(M.w3&&!(M.wk&&M.wk<312)&&ab&&ah&&ae&&ag&&Y){w(ah,false);K(function(){ae+="";ag+="";var aj={};if(af&&typeof af===r){for(var al in af){aj[al]=af[al]}}aj.data=ab;aj.width=ae;aj.height=ag;var am={};if(ad&&typeof ad===r){for(var ak in ad){am[ak]=ad[ak]}}if(Z&&typeof Z===r){for(var ai in Z){if(typeof am.flashvars!=D){am.flashvars+="&"+ai+"="+Z[ai]}else{am.flashvars=ai+"="+Z[ai]}}}if(F(Y)){var an=u(aj,am,ah);if(aj.id==ah){w(ah,true)}X.success=true;X.ref=an}else{if(aa&&A()){aj.data=aa;P(aj,am,ah,ac);return}else{w(ah,true)}}if(ac){ac(X)}})}else{if(ac){ac(X)}}},switchOffAutoHideShow:function(){m=false},ua:M,getFlashPlayerVersion:function(){return{major:M.pv[0],minor:M.pv[1],release:M.pv[2]}},hasFlashPlayerVersion:F,createSWF:function(Z,Y,X){if(M.w3){return u(Z,Y,X)}else{return undefined}},showExpressInstall:function(Z,aa,X,Y){if(M.w3&&A()){P(Z,aa,X,Y)}},removeSWF:function(X){if(M.w3){y(X)}},createCSS:function(aa,Z,Y,X){if(M.w3){v(aa,Z,Y,X)}},addDomLoadEvent:K,addLoadEvent:s,getQueryParamValue:function(aa){var Z=j.location.search||j.location.hash;if(Z){if(/\?/.test(Z)){Z=Z.split("?")[1]}if(aa==null){return L(Z)}var Y=Z.split("&");for(var X=0;X<Y.length;X++){if(Y[X].substring(0,Y[X].indexOf("="))==aa){return L(Y[X].substring((Y[X].indexOf("=")+1)))}}}return""},expressInstallCallback:function(){if(a){var X=c(R);if(X&&l){X.parentNode.replaceChild(l,X);if(Q){w(Q,true);if(M.ie&&M.win){l.style.display="block"}}if(E){E(B)}}a=false}}}}();

(function($){
    // 根据上传组件id和文件在队列中的index查找文件ID
    function _findIDByIndex(uploadId,index){
        var queueId = uploadId + 'Queue';
        var queueSize = $('#' + queueId + ' .om-fileupload-queueitem').length;
        if(index == null || isNaN(index) || index < 0 || index >= queueSize){
            return false;
        }
        var item = $('#' + queueId + ' .om-fileupload-queueitem:eq('+index+')');
        return item.attr('id').replace(uploadId,'');                
    };
    
    $.omWidget('om.omFileUpload', {
        options : /** @lends omFileUpload#*/{
            /**
             * 设置处理上传的swf文件的位置。
             * @default '/operamasks-ui/ui/om-fileupload.swf'
             * @type String
             * @example
             * $('#file_upload').omFileUpload({swf : 'om-fileupload.swf'});
             */
            swf : '/operamasks-ui/ui/om-fileupload.swf',
            /**
             * 处理文件上传的服务端地址。
             * @default 无
             * @type String
             * @example
             * $('#file_upload').omFileUpload({action : '/operamasks-ui/omFileUpload'});
             */
            action : '',
            /**
             * 设置上传到服务端的附加数据。使用这个属性的时候必须把method设置为'GET'。
             * @default 无
             * @type Object
             * @example
             * $('#file_upload').omFileUpload({method:'GET', actionData : {'name':'operamasks','age':'5'}});
             */
            actionData : {},
            /**
             * 设置上传按钮的高度。 
             * @default 30
             * @type Number
             * @example
             * $('#file_upload').omFileUpload({height : 50});
             */         
            height : 30,
            /**
             * 设置上传按钮的宽度。 
             * @default 120
             * @type Number
             * @example
             * $('#file_upload').omFileUpload({width : 150});
             */         
            width : 120,
            /**
             * 上传按钮的文字。 
             * @default '选择文件'
             * @type String
             * @example
             * $('#file_upload').omFileUpload({buttonText: '选择图片'});
             */         
            // buttonText : $.om.lang.omFileUpload.selectFileText,
            
            /**
             * 上传按钮的背景图片。
             * @default null(swf内置图片)
             * @type String
             * @example
             * $('#file_upload').omFileUpload({buttonImg: 'btn.jpg'});
             */
            buttonImg : null,
            
            /**
             * 是否允许批量上传文件。
             * @default false
             * @type Boolean
             * @example
             * $('#file_upload').omFileUpload({multi: true});
             */         
            multi : false,
            
            /**
             * 是否在选择完文件后自动执行上传。 
             * @default false
             * @type Boolean
             * @example
             * $('#file_upload').omFileUpload({autoUpload: true});
             */         
            autoUpload : false,
            fileDataName : 'Filedata',
            /**
             * 文件上传的表单提交方式。 
             * @default 'POST'
             * @type String
             * @example
             * $('#file_upload').omFileUpload({method: 'GET'});
             */             
            method : 'POST',
            /**
             * 批量上传文件数量的最大限制。
             * @default 999
             * @type Number
             * @example
             * $('#file_upload').omFileUpload({queueSizeLimit : 5});
             */         
            queueSizeLimit : 999,
            /**
             * 文件上传完成后是否自动移除上传的状态提示框。如果设置false则文件上传完后需要点击提示框的关闭按钮进行关闭。
             * @default true
             * @type Boolean
             * @example
             * $('#file_upload').omFileUpload({removeCompleted : false});
             */         
            removeCompleted : true,
            /**
             * 上传文件的类型限制，这个属性必须和fileDesc属性一起使用。 
             * @default '*.*'
             * @type String
             * @example
             * $('#file_upload').omFileUpload({fileExt : '*.jpg;*.png;*.gif',fileDesc:'Image Files'});
             */         
            fileExt : '*.*',
            /**
             * 在选择文件的弹出窗口中“文件类型”下拉框中显示的文字。
             * @default null
             * @type String
             * @example
             * $('#file_upload').omFileUpload({fileExt : '*.jpg;*.png;*.gif',fileDesc:'Image Files'});
             */         
            fileDesc : null,
            /**
             * 上传文件的最大限制。 
             * @default null(无大小限制)
             * @type Number
             * @example
             * $('#file_upload').omFileUpload({sizeLimit : 1024});
             */         
            sizeLimit : null,
            /**
             * 选择上传文件后触发。
             * @event
             * @param ID 文件ID
             * @param fileObj 封装了文件信息的Object对象，包含五个属性：name(文件名)，size(文件大小)，creationDate(文件创建时间)，modificationDate(文件最后修改时间)，type(文件类型)
             * @param event jQuery.Event对象。
             * @type Function
             * @default emptyFn
             * @name omFileUpload#onSelect
             * @example
             * $('#file_upload').omFileUpload({onSelect:function(ID,fileObj,event){alert('你选择了文件：'+fileObj.name);}});
             */         
            onSelect : function() {},
            /**
             * 批量上传的文件数量超过限制后触发。
             * @event
             * @param queueSizeLimit 批量上传文件数量的最大限制
             * @param event jQuery.Event对象。
             * @type Function
             * @default emptyFn
             * @name omFileUpload#onQueueFull
             * @example
             * $('#file_upload').omFileUpload({onSelect:function(queueSizeLimit,event){alert('批量上传文件的数量不能超过：'+queueSizeLimit);}});
             */             
            onQueueFull : function() {},
            /**
             * 选择上传文件后触发。
             * @event
             * @param ID 文件ID
             * @param fileObj 封装了文件的信息的Object对象，包含五个属性：name(文件名)，size(文件大小)，creationDate(文件创建时间)，modificationDate(文件最后修改时间)，type(文件类型)
             * @param data 封装了文件上传信息的Object对象，包含两个属性：fileCount(文件上传队列中剩余文件的数量)，speed(文件上传的平均速度 KB/s)
             * @param event jQuery.Event对象。
             * @type Function
             * @default emptyFn
             * @name omFileUpload#onCancel
             * @example
             * $('#file_upload').omFileUpload({onCalcel:function(ID,fileObj,data,event){alert('取消上传：'+fileObj.name);}});
             */         
            onCancel : function() {},
            /**
             * 文件上传出错后触发。
             * @event
             * @param ID 文件ID
             * @param fileObj 封装了文件的信息的Object对象，包含五个属性：name(文件名)，size(文件大小)，creationDate(文件创建时间)，modificationDate(文件最后修改时间)，type(文件类型)
             * @param errorObj 封装了返回的出错信息的Object对象，包含两个属性：type('HTTP'或'IO'或'Security')，info(返回的错误信息描述)
             * @param event jQuery.Event对象。
             * @type Function
             * @default emptyFn
             * @name omFileUpload#onError
             * @example
             * $('#file_upload').omFileUpload({onError:function(ID,fileObj,errorObj,event){alert('文件'+fileObj.name+'上传失败。错误类型：'+errorObj.type+'。原因：'+errorObj.info);}});
             */             
            onError : function() {},
            /**
             * 每次更新文件的上传进度后触发。
             * @event
             * @param ID 文件ID
             * @param fileObj 封装了文件的信息的Object对象，包含五个属性：name(文件名)，size(文件大小)，creationDate(文件创建时间)，modificationDate(文件最后修改时间)，type(文件类型)
             * @param data 封装了文件上传信息的Object对象，包含两个属性：fileCount(文件上传队列中剩余文件的数量)，speed(文件上传的平均速度 KB/s)
             * @param event jQuery.Event对象。
             * @type Function
             * @default emptyFn
             * @name omFileUpload#onProgress
             * @example
             * $('#file_upload').omFileUpload({onProgress:function(ID,fileObj,data,event){alert(fileObj.name+'上传平均速度：'+data.speed);}});
             */             
            onProgress : function() {},
            /**
             * 每个文件完成上传后触发。
             * @event
             * @param ID 文件ID
             * @param fileObj 封装了文件的信息的Object对象，包含五个属性：name(文件名)，size(文件大小)，creationDate(文件创建时间)，modificationDate(文件最后修改时间)，type(文件类型)
             * @param response 服务端返回的内容
             * @param data 封装了文件上传信息的Object对象，包含两个属性：fileCount(文件上传队列中剩余文件的数量)，speed(文件上传的平均速度 KB/s)
             * @param event jQuery.Event对象。
             * @type Function
             * @default emptyFn
             * @name omFileUpload#onComplete
             * @example
             * $('#file_upload').omFileUpload({onComplete:function(ID,fileObj,response,data,event){alert(fileObj.name+'上传完成');}});
             */             
            onComplete : function() {},
            /**
             * 所有文件上传完后触发。
             * @event
             * @param data 封装了文件上传信息的Object对象，包含两个属性：fileCount(文件上传队列中剩余文件的数量)，speed(文件上传的平均速度 KB/s)
             * @param event jQuery.Event对象。
             * @type Function
             * @default emptyFn
             * @name omFileUpload#onAllComplete
             * @example
             * $('#file_upload').omFileUpload({onAllComplete:function(data,event){alert('所有文件上传完毕');}});
             */             
            onAllComplete : function() {}
        },
    
    
        /**
         * 上传文件。如果不设置index参数则上传队列里面的所有文件。
         * @name omFileUpload#upload
         * @function
         * @param index 文件在上传队列中的索引，从0开始
         * @example
         * $('#file_upload').omFileUpload('upload'); // 上传队列中的所有文件
         * $('#file_upload').omFileUpload('upload',1); // 上传队列中的第2个文件
         */                 
        upload:function(index) {
            var element = this.element;
            var id = element.attr('id'),
            fileId = null,
            queueId = element.attr('id') + 'Queue',
            uploaderId = element.attr('id') + 'Uploader';
            if(typeof(index) != 'undefined'){
                if((fileId = _findIDByIndex(id,index)) === false) return;
            }
            document.getElementById(uploaderId).startFileUpload(fileId, false);
        },
        /**
         * 取消上传文件。如果不设置index参数则取消队列里面的所有文件。
         * @name omFileUpload#cancel
         * @function
         * @param index 文件在上传队列中的索引，从0开始
         * @example
         * $('#file_upload').omFileUpload('cancel'); // 取消上传队列中的所有文件
         * $('#file_upload').omFileUpload('cancel',1); // 取消上传队列中的第2个文件
         */                 
        cancel:function(index) {
            var element = this.element;
            var id = element.attr('id'),
            fileId = null,
            queueId = element.attr('id') + 'Queue',
            uploaderId = element.attr('id') + 'Uploader';
            if(typeof(index) != 'undefined'){
                // 增加对index参数为ID的情况的处理
                if(isNaN(index)){
                    fileId = index;
                } else{
                    if((fileId = _findIDByIndex(element.attr('id'),index)) === false) return;
                }
                document.getElementById(uploaderId).cancelFileUpload(fileId, true, true, false);
            } else{
                // cancel all
                document.getElementById(uploaderId).clearFileUploadQueue(false);
            }
        },
        
        _setOption : function(key, value) {
            var uploader = document.getElementById(this.element.attr('id') + 'Uploader');
            if (key == 'actionData') {
                var actionDataString = '';
                for (var name in value) {
                    actionDataString += '&' + name + '=' + value[name];
                }
                
                var cookieArray = document.cookie.split(';')
                for (var i = 0; i < cookieArray.length; i++){
                    if (cookieArray[i] !== '') {
                        actionDataString += '&' + cookieArray[i];
                    }
                }
                
                value = encodeURI(actionDataString.substr(1));
                uploader.updateSettings(key, value);
                return;
            }
            var dynOpts = ['buttonImg','buttonText','fileDesc','fileExt','height','action','sizeLimit','width'];
            if($.inArray(key, dynOpts) != -1){
                uploader.updateSettings(key, value);
            }
        }, 
        
        _create : function() {
        	var self = this;
            var element = this.element;
            var settings = $.extend({}, this.options);
            // 内置属性，不公布
            settings.wmode = 'opaque'; // The wmode of the flash file
            settings.expressInstall = null;
            settings.displayData = 'percentage';
            settings.folder = ''; // The path to the upload folder
            settings.simUploadLimit = 1; // The number of simultaneous uploads allowed
            settings.scriptAccess = 'sameDomain'; // Set to "always" to allow script access across domains
            settings.queueID = false; // The optional ID of the queue container
            settings.onInit = function(){}; // Function to run when omFileUpload is initialized
            settings.onSelectOnce = function(){}; // Function to run once when files are added to the queue
            settings.onClearQueue = function(){}; // Function to run when the queue is manually cleared
            settings.id = this.element.attr('id');
            
            $(element).data('settings',settings);
            var pagePath = location.pathname;
            pagePath = pagePath.split('/');
            pagePath.pop();
            pagePath = pagePath.join('/') + '/';
            var data = {};
            data.omFileUploadID = settings.id;
            
            data.pagepath = pagePath;
            if (settings.buttonImg) data.buttonImg = escape(settings.buttonImg);
            data.buttonText = encodeURI($.om.lang._get(settings,"omFileUpload","buttonText"));
            if (settings.rollover) data.rollover = true;
            data.action = settings.action;
            data.folder = escape(settings.folder);
            
            var actionDataString = '';
            var cookieArray = document.cookie.split(';')
            for (var i = 0; i < cookieArray.length; i++){
                actionDataString += '&' + cookieArray[i];
            }
            if (settings.actionData) {
                for (var name in settings.actionData) {
                    actionDataString += '&' + name + '=' + settings.actionData[name];
                }
            }
            data.actionData = escape(encodeURI(actionDataString.substr(1)));
            data.width = settings.width;
            data.height = settings.height;
            data.wmode = settings.wmode;
            data.method = settings.method;
            data.queueSizeLimit = settings.queueSizeLimit;
            data.simUploadLimit = settings.simUploadLimit;
            if (settings.hideButton) data.hideButton = true;
            if (settings.fileDesc) data.fileDesc = settings.fileDesc;
            if (settings.fileExt) data.fileExt = settings.fileExt;
            if (settings.multi) data.multi = true;
            if (settings.autoUpload) data.autoUpload = true;
            if (settings.sizeLimit) data.sizeLimit = settings.sizeLimit;
            if (settings.checkScript) data.checkScript = settings.checkScript;
            if (settings.fileDataName) data.fileDataName = settings.fileDataName;
            if (settings.queueID) data.queueID = settings.queueID;
            if (settings.onInit() !== false) {
                element.css('display','none');
                element.after('<div id="' + element.attr('id') + 'Uploader"></div>');
                swfobject.embedSWF(settings.swf, settings.id + 'Uploader', settings.width, settings.height, '9.0.24', settings.expressInstall, data, {'quality':'high','wmode':settings.wmode,'allowScriptAccess':settings.scriptAccess},{},function(event) {
                    if (typeof(settings.onSWFReady) == 'function' && event.success) settings.onSWFReady();
                });
                if (settings.queueID == false) {
                    $("#" + element.attr('id') + "Uploader").after('<div id="' + element.attr('id') + 'Queue" class="om-fileupload-queue"></div>');
                } else {
                    $("#" + settings.queueID).addClass('om-fileupload-queue');
                }
            }
            if (typeof(settings.onOpen) == 'function') {
                element.bind("omFileUploadOpen", settings.onOpen);
            }
            element.bind("omFileUploadSelect", {'action': settings.onSelect, 'queueID': settings.queueID}, function(event, ID, fileObj) {
                if (self._trigger("onSelect",event,ID,fileObj) !== false) {
                    var byteSize = Math.round(fileObj.size / 1024 * 100) * .01;
                    var suffix = 'KB';
                    if (byteSize > 1000) {
                        byteSize = Math.round(byteSize *.001 * 100) * .01;
                        suffix = 'MB';
                    }
                    var sizeParts = byteSize.toString().split('.');
                    if (sizeParts.length > 1) {
                        byteSize = sizeParts[0] + '.' + sizeParts[1].substr(0,2);
                    } else {
                        byteSize = sizeParts[0];
                    }
                    if (fileObj.name.length > 20) {
                        fileName = fileObj.name.substr(0,20) + '...';
                    } else {
                        fileName = fileObj.name;
                    }
                    queue = '#' + $(this).attr('id') + 'Queue';
                    if (event.data.queueID) {
                        queue = '#' + event.data.queueID;
                    }
                    $(queue).append('<div id="' + $(this).attr('id') + ID + '" class="om-fileupload-queueitem">\
                            <div class="cancel" onclick="$(\'#' + $(this).attr('id') + '\').omFileUpload(\'cancel\',\'' + ID + '\')">\
                            </div>\
                            <span class="fileName">' + fileName + ' (' + byteSize + suffix + ')</span><span class="percentage"></span>\
                            <div class="om-fileupload-progress">\
                                <div id="' + $(this).attr('id') + ID + 'ProgressBar" class="om-fileupload-progressbar"><!--Progress Bar--></div>\
                            </div>\
                        </div>');
                }
            });
            element.bind("omFileUploadSelectOnce", {'action': settings.onSelectOnce}, function(event, data) {
            	self._trigger("onSelectOnce",event,data);
                if (settings.autoUpload) {
                    $(this).omFileUpload('upload');
                }
            });
            element.bind("omFileUploadQueueFull", {'action': settings.onQueueFull}, function(event, queueSizeLimit) {
                if (self._trigger("onQueueFull",event,queueSizeLimit) !== false) {
                    alert($.om.lang.omFileUpload.queueSizeLimitMsg + queueSizeLimit + '.');
                }
            });
            element.bind("omFileUploadCancel", {'action': settings.onCancel}, function(event, ID, fileObj, data, remove, clearFast) {
                if (self._trigger("onCancel",event,ID,fileObj,data) !== false) {
                    if (remove) { 
                        var fadeSpeed = (clearFast == true) ? 0 : 250;
                        $("#" + $(this).attr('id') + ID).fadeOut(fadeSpeed, function() { $(this).remove() });
                    }
                }
            });
            element.bind("omFileUploadClearQueue", {'action': settings.onClearQueue}, function(event, clearFast) {
                var queueID = (settings.queueID) ? settings.queueID : $(this).attr('id') + 'Queue';
                if (clearFast) {
                    $("#" + queueID).find('.om-fileupload-queueitem').remove();
                }
                if (self._trigger("onClearQueue",event,clearFast) !== false) {
                    $("#" + queueID).find('.om-fileupload-queueitem').each(function() {
                        var index = $('.om-fileupload-queueitem').index(this);
                        $(this).delay(index * 100).fadeOut(250, function() { $(this).remove() });
                    });
                }
            });
            var errorArray = [];
            element.bind("omFileUploadError", {'action': settings.onError}, function(event, ID, fileObj, errorObj) {
                if (self._trigger("onError",event,ID,fileObj,errorObj) !== false) {
                    var fileArray = new Array(ID, fileObj, errorObj);
                    errorArray.push(fileArray);
                    $("#" + $(this).attr('id') + ID).find('.percentage').text(" - " + errorObj.type + " Error");
                    $("#" + $(this).attr('id') + ID).find('.om-fileupload-progress').hide();
                    $("#" + $(this).attr('id') + ID).addClass('om-fileupload-error');
                }
            });
            if (typeof(settings.onUpload) == 'function') {
                element.bind("omFileUploadUpload", settings.onUpload);
            }
            element.bind("omFileUploadProgress", {'action': settings.onProgress, 'toDisplay': settings.displayData}, function(event, ID, fileObj, data) {
                if (self._trigger("onProgress",event,ID,fileObj,data) !== false) {
                    $("#" + $(this).attr('id') + ID + "ProgressBar").animate({'width': data.percentage + '%'},250,function() {
                        if (data.percentage == 100) {
                            $(this).closest('.om-fileupload-progress').fadeOut(250,function() {$(this).remove()});
                        }
                    });
                    if (event.data.toDisplay == 'percentage') displayData = ' - ' + data.percentage + '%';
                    if (event.data.toDisplay == 'speed') displayData = ' - ' + data.speed + 'KB/s';
                    if (event.data.toDisplay == null) displayData = ' ';
                    $("#" + $(this).attr('id') + ID).find('.percentage').text(displayData);
                }
            });
            element.bind("omFileUploadComplete", {'action': settings.onComplete}, function(event, ID, fileObj, response, data) {
                if (self._trigger("onComplete",event,ID,fileObj,unescape(response),data) !== false) {
                    $("#" + $(this).attr('id') + ID).find('.percentage').text(' - Completed');
                    if (settings.removeCompleted) {
                        $("#" + $(event.target).attr('id') + ID).fadeOut(250,function() {$(this).remove()});
                    }
                    $("#" + $(event.target).attr('id') + ID).addClass('completed');
                }
            });
            if (typeof(settings.onAllComplete) == 'function') {
                element.bind("omFileUploadAllComplete", {'action': settings.onAllComplete}, function(event, data) {
                    if (self._trigger("onAllComplete",event,data) !== false) {
                        errorArray = [];
                    }
                });
            }
        }
    });
    
    $.om.lang.omFileUpload = {
        queueSizeLimitMsg:'文件上传队列已满，数量不能超过',
        buttonText:'选择文件'
    };
})(jQuery);