// 
// PlugRect.cs
//  
// Author: Daniele Giardini + Romain Giraud
// 
// Copyright (c) 2012 Daniele Giardini - Holoville - http://www.holoville.com
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

// Original PlugRect code contributed by Romain Giraud

using System;
using UnityEngine;

namespace Holoville.HOTween.Plugins.Core
{
	/// <summary>
	/// Default plugin for the tweening of Rect objects.
	/// </summary>
	public class PlugRect : ABSTweenPlugin
	{
		// VARS ///////////////////////////////////////////////////
		
		static internal Type[]			validPropTypes = { typeof(Rect) };
		static internal	Type[]			validValueTypes = { typeof(Rect) };
		
		private		Rect				typedStartVal;
		private		Rect				typedEndVal;
		private		float				changeVal;
		private		Rect				diffChangeVal; // Used for incremental loops.
		
		// GETS/SETS //////////////////////////////////////////////
		
		/// <summary>
		/// Gets the untyped start value,
		/// sets both the untyped and the typed start value.
		/// </summary>
		override protected	object		startVal {
			get { return _startVal; }
			set {
				if ( tweenObj.isFrom && isRelative ) {
					typedStartVal = (Rect)value;
					typedStartVal.x += typedEndVal.x;
					typedStartVal.y += typedEndVal.y;
					typedStartVal.width += typedEndVal.width;
					typedStartVal.height += typedEndVal.height;
					_startVal = typedStartVal;
				} else {
					_startVal = typedStartVal = (Rect)value;
				}
			}
		}
		
		/// <summary>
		/// Gets the untyped end value,
		/// sets both the untyped and the typed end value.
		/// </summary>
		override protected	object		endVal {
			get { return _endVal; }
			set { _endVal = typedEndVal = (Rect)value; }
		}
		
		
		// ***********************************************************************************
		// CONSTRUCTOR
		// ***********************************************************************************
		
		/// <summary>
		/// Creates a new instance of this plugin using the main ease type.
		/// </summary>
		/// <param name="p_endVal">
		/// The <see cref="Rect"/> value to tween to.
		/// </param>
		public PlugRect( Rect p_endVal ) : base( p_endVal, false ) {}
		/// <summary>
		/// Creates a new instance of this plugin.
		/// </summary>
		/// <param name="p_endVal">
		/// The <see cref="Rect"/> value to tween to.
		/// </param>
		/// <param name="p_easeType">
		/// The <see cref="EaseType"/> to use.
		/// </param>
		public PlugRect( Rect p_endVal, EaseType p_easeType ) : base( p_endVal, p_easeType, false ) {}
		/// <summary>
		/// Creates a new instance of this plugin using the main ease type.
		/// </summary>
		/// <param name="p_endVal">
		/// The <see cref="Rect"/> value to tween to.
		/// </param>
		/// <param name="p_isRelative">
		/// If <c>true</c>, the given end value is considered relative instead than absolute.
		/// </param>
		public PlugRect( Rect p_endVal, bool p_isRelative ) : base( p_endVal, p_isRelative ) {}
		/// <summary>
		/// Creates a new instance of this plugin.
		/// </summary>
		/// <param name="p_endVal">
		/// The <see cref="Rect"/> value to tween to.
		/// </param>
		/// <param name="p_easeType">
		/// The <see cref="EaseType"/> to use.
		/// </param>
		/// <param name="p_isRelative">
		/// If <c>true</c>, the given end value is considered relative instead than absolute.
		/// </param>
		public PlugRect( Rect p_endVal, EaseType p_easeType, bool p_isRelative ) : base( p_endVal, p_easeType, p_isRelative ) {}
		
		// ===================================================================================
		// METHODS ---------------------------------------------------------------------------
		
		/// <summary>
		/// Returns the speed-based duration based on the given speed x second.
		/// </summary>
		override protected float GetSpeedBasedDuration( float p_speed )
		{
			// Uses length of diagonal to calculate units.
			float diffW = typedEndVal.width - typedStartVal.width;
			float diffH = typedEndVal.height - typedStartVal.height;
			float diag = Mathf.Sqrt( diffW * diffW + diffH * diffH );
			float speedDur = diag / p_speed;
			if ( speedDur < 0 )		speedDur = -speedDur;
			return speedDur;
		}
		
		/// <summary>
		/// Sets the typed changeVal based on the current startVal and endVal.
		/// </summary>
		override protected void SetChangeVal()
		{
			changeVal = 1;
			
			if ( isRelative && !tweenObj.isFrom ) {
				typedEndVal.x += typedStartVal.x;
				typedEndVal.y += typedStartVal.y;
				typedEndVal.width += typedStartVal.width;
				typedEndVal.height += typedStartVal.height;
			}
			
			diffChangeVal = new Rect();
			diffChangeVal.x = typedEndVal.x - typedStartVal.x;
			diffChangeVal.y = typedEndVal.y - typedStartVal.y;
			diffChangeVal.width = typedEndVal.width - typedStartVal.width;
			diffChangeVal.height = typedEndVal.height - typedStartVal.height;
		}
		
		/// <summary>
		/// Sets the correct values in case of Incremental loop type.
		/// </summary>
		/// <param name="p_diffIncr">
		/// The difference from the previous loop increment.
		/// </param>
		override internal void SetIncremental( int p_diffIncr )
		{
			Rect diffR = new Rect( diffChangeVal.x, diffChangeVal.y, diffChangeVal.width, diffChangeVal.height );
			diffR.x *= p_diffIncr;
			diffR.y *= p_diffIncr;
			diffR.width *= p_diffIncr;
			diffR.height *= p_diffIncr;
			
			typedStartVal.x += diffR.x;
			typedStartVal.y += diffR.y;
			typedStartVal.width += diffR.width;
			typedStartVal.height += diffR.height;
			
			typedEndVal.x += diffR.x;
			typedEndVal.y += diffR.y;
			typedEndVal.width += diffR.width;
			typedEndVal.height += diffR.height;
		}
		
		/// <summary>
		/// Updates the tween.
		/// </summary>
		/// <param name="p_totElapsed">
		/// The total elapsed time since startup.
		/// </param>
		override protected void DoUpdate ( float p_totElapsed )
		{
			float val = ease( p_totElapsed, 0, changeVal, _duration );
			
			Rect r = new Rect();
			r.x = Mathf.Lerp( typedStartVal.x, typedEndVal.x, val );
			r.y = Mathf.Lerp( typedStartVal.y, typedEndVal.y, val );
			r.width  = Mathf.Lerp( typedStartVal.width, typedEndVal.width, val );
			r.height = Mathf.Lerp( typedStartVal.height, typedEndVal.height, val );
			
			SetValue( r );
		}
	}
}
