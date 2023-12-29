using JetBrains.Annotations;
using UnityEngine;

namespace PrimeTween {
    [PublicAPI]
    public readonly struct Easing {
        internal readonly Ease ease;
        internal readonly AnimationCurve curve;
        internal readonly ParametricEase parametricEase;
        internal readonly float parametricEaseStrength;
        internal readonly float parametricEasePeriod;

        Easing(ParametricEase type, float strength, float period = float.NaN) {
            ease = Ease.Custom;
            curve = null;
            parametricEase = type;
            parametricEaseStrength = strength;
            parametricEasePeriod = period;
        }

        Easing(Ease ease, AnimationCurve curve) {
            this.ease = ease;
            this.curve = curve;
            parametricEase = ParametricEase.None;
            parametricEaseStrength = float.NaN;
            parametricEasePeriod = float.NaN;
        }
        
        public static implicit operator Easing(Ease ease) => Standard(ease);
        /// <summary>Standard Robert Penner's easing methods. Or simply use Ease enum instead of this method.</summary>
        public static Easing Standard(Ease ease) {
            Assert.AreNotEqual(Ease.Custom, ease);
            if (ease == Ease.Default) {
                ease = PrimeTweenConfig.defaultEase;
            }
            return new Easing(ease, null);
        }

        public static implicit operator Easing([NotNull] AnimationCurve curve) => Curve(curve);
        /// <summary>AnimationCurve to use an easing function. Or simply use AnimationCurve instead of this method.</summary>
        public static Easing Curve([NotNull] AnimationCurve curve) => new Easing(Ease.Custom, curve);

        #if PRIME_TWEEN_EXPERIMENTAL
        public static Easing Bounce(float strength) => new Easing(ParametricEase.Bounce, strength);
        /// <summary>The first bounce will have the exact <see cref="amplitude"/> measured in meters/angles.</summary>
        public static Easing BounceExact(float amplitude) => new Easing(ParametricEase.BounceExact, amplitude);
        public static Easing Overshoot(float strength) => new Easing(ParametricEase.Overshoot, strength * StandardEasing.backEaseConst);
        public static Easing Elastic(float strength, float period = 0.3f) {
            if (strength < 1) {
                strength = Mathf.Lerp(0.2f, 1f, strength); // remap strength to limit decayFactor
            }
            return new Easing(ParametricEase.Elastic, strength, Mathf.Max(0.1f, period));
        }
        #endif

        internal static float Evaluate(float t, [NotNull] ReusableTween tween) {
            var settings = tween.settings;
            var strength = settings.parametricEaseStrength;
            var period = settings.parametricEasePeriod;
            switch (settings.parametricEase) {
                case ParametricEase.Overshoot:
                    t -= 1.0f;
                    return t * t * ((strength + 1) * t + strength) + 1.0f;
                case ParametricEase.Elastic:
                    const float twoPi = 2 * Mathf.PI;
                    float decayFactor;
                    if (strength >= 1) {
                        decayFactor = 1f;
                    } else {
                        decayFactor = 1 / strength;
                        strength = 1;
                    }
                    float decay = Mathf.Pow(2, -10f * t * decayFactor);
                    var duration = settings.duration;
                    if (duration == 0) {
                        return 1;
                    }
                    period /= duration;
                    float phase = period / twoPi * Mathf.Asin(1f / strength);
                    return t > 0.9999f ? 1 : strength * decay * Mathf.Sin((t - phase) * twoPi / period) + 1f;
                case ParametricEase.Bounce:
                    return Bounce(t, tween, 1);
                case ParametricEase.BounceExact:
                    var fullAmplitude = tween.propType == PropType.Quaternion ? 
                        Quaternion.Angle(tween.startValue.QuaternionVal, tween.endValue.QuaternionVal) : 
                        tween.diff.Vector4Val.magnitude;
                    var strengthFactor = fullAmplitude < 0.0001f ? 1 : 1f / (fullAmplitude * (1f - firstBounceAmpl));
                    return Bounce(t, tween, strengthFactor);
                case ParametricEase.None:
                default:
                    throw new System.Exception();
            }
        }

        const float firstBounceAmpl = 0.75f;
        static float Bounce(float t, [NotNull] ReusableTween tween, float strengthFactor) {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;
            if (t < 1 / d1) {
                return n1 * t * t;
            }
            return 1 - (1 - bounce()) * tween.settings.parametricEaseStrength * strengthFactor;
            float bounce() {
                if (t < 2 / d1) {
                    return n1 * (t -= 1.5f / d1) * t + firstBounceAmpl;
                }
                if (t < 2.5 / d1) {
                    return n1 * (t -= 2.25f / d1) * t + 0.9375f;
                }
                return n1 * (t -= 2.625f / d1) * t + 0.984375f;
            }
        }
    }

    internal enum ParametricEase {
        None = 0,
        Overshoot = 5,
        Bounce = 7,
        Elastic = 11,
        BounceExact
    }
}