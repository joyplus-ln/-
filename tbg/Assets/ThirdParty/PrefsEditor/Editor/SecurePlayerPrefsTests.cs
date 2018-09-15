//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016-2017 Flip Web Apps / Mark Hewitt
//
// Please direct any bugs/comments/suggestions to http://www.flipwebapps.com
// 
// The copyright owner grants to the end user a non-exclusive, worldwide, and perpetual license to this Asset
// to integrate only as incorporated and embedded components of electronic games and interactive media and 
// distribute such electronic game and interactive media. End user may modify Assets. End user may otherwise 
// not reproduce, distribute, sublicense, rent, lease or lend the Assets. It is emphasized that the end 
// user shall not be entitled to distribute or transfer in any way (including, without, limitation by way of 
// sublicense) the Assets in any other way than as integrated components of electronic games and interactive media. 

// The above copyright notice and this permission notice must not be removed from any files.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//----------------------------------------------

#if UNITY_5_0 || UNITY_5_1 || UNITY_5_2
#else
using NUnit.Framework;
using UnityEngine;

namespace PrefsEditor.Editor
{
    /// <summary>
    /// Test cases for SecurePlayerPrefs. You can also view these to see how you might use the API.
    /// </summary>
    public class SecurePlayerPrefsTests
    {

        #region EncryptKey Tests

        [TestCase("A Test Key")]
        [TestCase("AnotherKey")]
        [TestCase("KeyÅØÆèá法加")]
        public void EncryptKey(string value)
        {
            // Arrange

            // Act
            var encryptedKey = SecurePlayerPrefs.EncryptKey(value);
            var encryptedKey2 = SecurePlayerPrefs.EncryptKey(value);

            // Assert
            Assert.AreNotEqual(value, encryptedKey, "The value was not encrypted");
            Assert.AreEqual(encryptedKey, encryptedKey2, "The encrypted values are not the same");

            // Cleanup
        }
        #endregion EncryptKey Tests

        #region HasKey

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void HasKeyNoKeySet(int value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = false;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            SecurePlayerPrefs.DeleteKey("TestInt" + value);

            // Act

            // Assert
            Assert.IsFalse(SecurePlayerPrefs.HasKey("TestInt" + value), "The key should not have been found");

            // Cleanup
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void HasKeyNoKeySetEncrypted(int value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            SecurePlayerPrefs.DeleteKey("TestInt" + value);

            // Act

            // Assert
            Assert.IsFalse(SecurePlayerPrefs.HasKey("TestInt" + value), "The key should not have been found");

            // Cleanup
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void HasKey(int value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = false;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            SecurePlayerPrefs.DeleteKey("TestInt" + value);
            SecurePlayerPrefs.SetInt("TestInt" + value, 1);

            // Act

            // Assert
            Assert.IsTrue(SecurePlayerPrefs.HasKey("TestInt" + value), "The key should have been found");

            // Cleanup
            SecurePlayerPrefs.DeleteKey("TestInt" + value);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void HasKeyEncrypted(int value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            SecurePlayerPrefs.DeleteKey("TestInt" + value);
            SecurePlayerPrefs.SetInt("TestInt" + value, 1);

            // Act

            // Assert
            Assert.IsFalse(PlayerPrefs.HasKey("TestInt" + value), "The keys was not encrypted");
            Assert.IsTrue(SecurePlayerPrefs.HasKey("TestInt" + value), "The keys was not found");

            // Cleanup
            SecurePlayerPrefs.DeleteKey("TestInt" + value);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void HasKeyAutoConvert(int value)
        {
            // Arrange
            PlayerPrefs.DeleteKey("TestInt" + value);
            PlayerPrefs.SetInt("TestInt" + value, 1);
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = true;

            // Act

            // Assert
            Assert.IsTrue(SecurePlayerPrefs.HasKey("TestInt" + value), "The key was not found");

            // Cleanup
            PlayerPrefs.DeleteKey("TestInt" + value);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void HasKeyNoAutoConvert(int value)
        {
            // Arrange
            PlayerPrefs.DeleteKey("TestInt" + value);
            PlayerPrefs.SetInt("TestInt" + value, 1);
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;

            // Act

            // Assert
            Assert.IsFalse(SecurePlayerPrefs.HasKey("TestInt" + value), "The key was not found");

            // Cleanup
            PlayerPrefs.DeleteKey("TestInt" + value);
        }

        #endregion

        #region Delete

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void DeleteKey(int value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = false;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            SecurePlayerPrefs.SetInt("TestInt" + value, value);

            // Act
            SecurePlayerPrefs.DeleteKey("TestInt" + value);

            // Assert
            Assert.IsFalse(SecurePlayerPrefs.HasKey("TestInt" + value), "The key was not removed");

            // Cleanup
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void DeleteKeyEncrypted(int value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            SecurePlayerPrefs.SetInt("TestInt" + value, value);

            // Act
            SecurePlayerPrefs.DeleteKey("TestInt" + value);

            // Assert
            Assert.IsFalse(SecurePlayerPrefs.HasKey("TestInt" + value), "The key was not removed");

            // Cleanup
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void DeleteKeyAutoConvert(int value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            PlayerPrefs.SetInt("TestInt" + value, value);
            SecurePlayerPrefs.SetInt("TestInt" + value, value);
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = true;

            // Act
            SecurePlayerPrefs.DeleteKey("TestInt" + value);

            // Assert
            Assert.IsFalse(PlayerPrefs.HasKey("TestInt" + value), "The standard key was not removed");
            Assert.IsFalse(SecurePlayerPrefs.HasKey("TestInt" + value), "The encrypted key was not removed");

            // Cleanup
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void DeleteKeyNoAutoConvert(int value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            PlayerPrefs.SetInt("TestInt" + value, value);
            SecurePlayerPrefs.SetInt("TestInt" + value, value);

            // Act
            SecurePlayerPrefs.DeleteKey("TestInt" + value);

            // Assert
            Assert.IsTrue(PlayerPrefs.HasKey("TestInt" + value), "The standard key was removed");
            Assert.IsFalse(SecurePlayerPrefs.HasKey("TestInt" + value), "The encrypted key was not removed");

            // Cleanup
            PlayerPrefs.DeleteKey("TestInt" + value);
        }

        [Test]
        public void DeleteAll()
        {
            // Arrange
            SecurePlayerPrefs.SetInt("TestInt", 1);
            SecurePlayerPrefs.SetFloat("TestFloat", 1.1f);
            SecurePlayerPrefs.SetString("TestString", "Test String");
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.SetString("TestStringEncrypted", "Test String");

            // Act
            SecurePlayerPrefs.DeleteAll();

            // Assert
            Assert.IsFalse(SecurePlayerPrefs.HasKey("TestInt") && SecurePlayerPrefs.HasKey("TestFloat") && SecurePlayerPrefs.HasKey("TestString") && SecurePlayerPrefs.HasKey("TestStringEncrypted"), "The keys were not removed");

            // Cleanup
        }

        #endregion

        #region Int Prefs Tests

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void GetInt(int value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = false;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            PlayerPrefs.DeleteKey("TestInt" + value);
            PlayerPrefs.SetInt("TestInt" + value, value);

            // Act

            // Assert
            Assert.AreEqual(value, SecurePlayerPrefs.GetInt("TestInt" + value), "The value was not set");

            // Cleanup
            PlayerPrefs.DeleteKey("TestInt" + value);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void SetInt(int value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = false;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            SecurePlayerPrefs.DeleteKey("TestInt" + value);
            SecurePlayerPrefs.SetInt("TestInt" + value, value);

            // Act

            // Assert
            Assert.AreEqual(value, PlayerPrefs.GetInt("TestInt" + value), "The value was not set");

            // Cleanup
            SecurePlayerPrefs.DeleteKey("TestInt" + value);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void SetIntEncrypted(int value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            SecurePlayerPrefs.DeleteKey("TestInt" + value);

            // Act
            SecurePlayerPrefs.SetInt("TestInt" + value, value);

            // Assert
            Assert.IsNotNull(SecurePlayerPrefs.GetRawEncryptedPrefsEntry("TestInt" + value), "The value was null");
            Assert.AreNotEqual(value.ToString(), SecurePlayerPrefs.GetRawEncryptedPrefsEntry("TestInt" + value), "The value was not set encrypted");

            // Cleanup
            SecurePlayerPrefs.DeleteKey("TestInt" + value);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void GetIntEncrypted(int value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            SecurePlayerPrefs.DeleteKey("TestInt" + value);

            // Act
            SecurePlayerPrefs.SetInt("TestInt" + value, value);

            // Assert
            Assert.AreEqual(value, SecurePlayerPrefs.GetInt("TestInt" + value), "The value could not be decrypted");

            // Cleanup
            SecurePlayerPrefs.DeleteKey("TestInt" + value);
        }


        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void GetIntAutoConvert(int value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = true;
            SecurePlayerPrefs.DeleteKey("TestInt" + value);
            PlayerPrefs.DeleteKey("TestInt" + value);
            PlayerPrefs.SetInt("TestInt" + value, value);

            // Act

            // Assert
            Assert.AreEqual(value, SecurePlayerPrefs.GetInt("TestInt" + value), "The value could not be decrypted");
            Assert.IsFalse(PlayerPrefs.HasKey("TestInt" + value), "The old converted key was not removed");

            // Cleanup
            SecurePlayerPrefs.DeleteKey("TestInt" + value);
            PlayerPrefs.DeleteKey("TestInt" + value);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void GetIntNoAutoConvert(int value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            SecurePlayerPrefs.DeleteKey("TestInt" + value);
            PlayerPrefs.DeleteKey("TestInt" + value);
            PlayerPrefs.SetInt("TestInt" + value, value);

            // Act

            // Assert
            Assert.AreNotEqual(value, SecurePlayerPrefs.GetInt("TestInt" + value), "The value was wrongly retrieved");
            Assert.IsTrue(PlayerPrefs.HasKey("TestInt" + value), "The old converted key was removed");

            // Cleanup
            SecurePlayerPrefs.DeleteKey("TestInt" + value);
            PlayerPrefs.DeleteKey("TestInt" + value);
        }

        #endregion Int Prefs Tests

        #region Bool Prefs Tests

        [TestCase(true)]
        [TestCase(false)]
        public void GetBool(bool value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = false;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            PlayerPrefs.DeleteKey("TestBool" + value);
            PlayerPrefs.SetInt("TestBool" + value, value ? 1 : 0);

            // Act

            // Assert
            Assert.AreEqual(value, SecurePlayerPrefs.GetBool("TestBool" + value), "The value was not set");

            // Cleanup
            PlayerPrefs.DeleteKey("TestBool" + value);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SetBool(bool value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = false;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            SecurePlayerPrefs.DeleteKey("TestBool" + value);
            SecurePlayerPrefs.SetBool("TestBool" + value, value);

            // Act

            // Assert
            Assert.AreEqual(value, PlayerPrefs.GetInt("TestBool" + value) == 1, "The value was not set");

            // Cleanup
            SecurePlayerPrefs.DeleteKey("TestBool" + value);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void SetBoolEncrypted(bool value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            SecurePlayerPrefs.DeleteKey("TestBool" + value);

            // Act
            SecurePlayerPrefs.SetBool("TestBool" + value, value);

            // Assert
            Assert.IsNotNull(SecurePlayerPrefs.GetRawEncryptedPrefsEntry("TestBool" + value), "The value was null");
            Assert.AreNotEqual(value.ToString(), SecurePlayerPrefs.GetRawEncryptedPrefsEntry("TestBool" + value), "The value was not set encrypted");

            // Cleanup
            SecurePlayerPrefs.DeleteKey("TestBool" + value);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetBoolEncrypted(bool value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            SecurePlayerPrefs.DeleteKey("TestBool" + value);

            // Act
            SecurePlayerPrefs.SetBool("TestBool" + value, value);

            // Assert
            Assert.AreEqual(value, SecurePlayerPrefs.GetBool("TestBool" + value), "The value could not be decrypted");

            // Cleanup
            SecurePlayerPrefs.DeleteKey("TestBool" + value);
        }


        [TestCase(true)]
        [TestCase(false)]
        public void GetBoolAutoConvert(bool value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = true;
            SecurePlayerPrefs.DeleteKey("TestBool" + value);
            PlayerPrefs.DeleteKey("TestBool" + value);
            PlayerPrefs.SetInt("TestBool" + value, value ? 1 : 0);

            // Act

            // Assert
            Assert.AreEqual(value, SecurePlayerPrefs.GetBool("TestBool" + value), "The value could not be decrypted");
            Assert.IsFalse(PlayerPrefs.HasKey("TestBool" + value), "The old converted key was not removed");

            // Cleanup
            SecurePlayerPrefs.DeleteKey("TestBool" + value);
            PlayerPrefs.DeleteKey("TestBool" + value);
        }

        [TestCase(true)]
        public void GetBoolNoAutoConvert(bool value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            SecurePlayerPrefs.DeleteKey("TestBool" + value);
            PlayerPrefs.DeleteKey("TestBool" + value);
            PlayerPrefs.SetInt("TestBool" + value, value ? 1 : 0);

            // Act

            // Assert
            Assert.AreNotEqual(value, SecurePlayerPrefs.GetBool("TestBool" + value), "The value was wrongly retrieved");
            Assert.IsTrue(PlayerPrefs.HasKey("TestBool" + value), "The old converted key was removed");

            // Cleanup
            SecurePlayerPrefs.DeleteKey("TestBool" + value);
            PlayerPrefs.DeleteKey("TestBool" + value);
        }

        #endregion Bool Prefs Tests

        #region Float Prefs Tests

        [TestCase(1.0f)]
        [TestCase(2.2f)]
        [TestCase(4.5f)]
        public void GetFloat(float value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = false;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            PlayerPrefs.DeleteKey("TestFloat" + value);
            PlayerPrefs.SetFloat("TestFloat" + value, value);

            // Act

            // Assert
            Assert.AreEqual(value, SecurePlayerPrefs.GetFloat("TestFloat" + value), "The value was not set");

            // Cleanup
            PlayerPrefs.DeleteKey("TestFloat" + value);
        }

        [TestCase(1.0f)]
        [TestCase(2.2f)]
        [TestCase(4.5f)]
        public void SetFloat(float value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = false;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            SecurePlayerPrefs.DeleteKey("TestFloat" + value);
            SecurePlayerPrefs.SetFloat("TestFloat" + value, value);

            // Act

            // Assert
            Assert.AreEqual(value, PlayerPrefs.GetFloat("TestFloat" + value), "The value was not set");

            // Cleanup
            SecurePlayerPrefs.DeleteKey("TestFloat" + value);
        }

        [TestCase(1.0f)]
        [TestCase(2.2f)]
        [TestCase(4.5f)]
        public void SetFloatEncrypted(float value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            SecurePlayerPrefs.DeleteKey("TestFloat" + value);

            // Act
            SecurePlayerPrefs.SetFloat("TestFloat" + value, value);

            // Assert
            Assert.IsNotNull(SecurePlayerPrefs.GetRawEncryptedPrefsEntry("TestFloat" + value), "The value was null");
            Assert.AreNotEqual(value.ToString(), SecurePlayerPrefs.GetRawEncryptedPrefsEntry("TestFloat" + value), "The value was not set encrypted");

            // Cleanup
            SecurePlayerPrefs.DeleteKey("TestFloat" + value);
        }

        [TestCase(1.0f)]
        [TestCase(2.2f)]
        [TestCase(4.5f)]
        public void GetFloatEncrypted(float value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            SecurePlayerPrefs.DeleteKey("TestFloat" + value);

            // Act
            SecurePlayerPrefs.SetFloat("TestFloat" + value, value);

            // Assert
            Assert.AreEqual(value, SecurePlayerPrefs.GetFloat("TestFloat" + value), "The value could not be decrypted");

            // Cleanup
            SecurePlayerPrefs.DeleteKey("TestFloat" + value);
        }


        [TestCase(1.0f)]
        [TestCase(2.2f)]
        [TestCase(4.5f)]
        public void GetFloatAutoConvert(float value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = true;
            SecurePlayerPrefs.DeleteKey("TestFloat" + value);
            PlayerPrefs.DeleteKey("TestFloat" + value);
            PlayerPrefs.SetFloat("TestFloat" + value, value);

            // Act

            // Assert
            Assert.AreEqual(value, SecurePlayerPrefs.GetFloat("TestFloat" + value), "The value could not be decrypted");
            Assert.IsFalse(PlayerPrefs.HasKey("TestFloat" + value), "The old converted key was not removed");

            // Cleanup
            SecurePlayerPrefs.DeleteKey("TestFloat" + value);
            PlayerPrefs.DeleteKey("TestFloat" + value);
        }

        [TestCase(1.0f)]
        [TestCase(2.2f)]
        [TestCase(4.5f)]
        public void GetFloatNoAutoConvert(float value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            SecurePlayerPrefs.DeleteKey("TestFloat" + value);
            PlayerPrefs.DeleteKey("TestFloat" + value);
            PlayerPrefs.SetFloat("TestFloat" + value, value);

            // Act

            // Assert
            Assert.AreNotEqual(value, SecurePlayerPrefs.GetFloat("TestFloat" + value), "The value was wrongly retrieved");
            Assert.IsTrue(PlayerPrefs.HasKey("TestFloat" + value), "The old converted key was removed");

            // Cleanup
            SecurePlayerPrefs.DeleteKey("TestFloat" + value);
            PlayerPrefs.DeleteKey("TestFloat" + value);
        }

        #endregion Float Prefs Tests

        #region String Prefs Tests

        [TestCase("Test String 1")]
        [TestCase("Another Test")]
        [TestCase("Third Test")]
        public void GetString(string value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = false;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            PlayerPrefs.DeleteKey("TestString" + value);
            PlayerPrefs.SetString("TestString" + value, value);

            // Act

            // Assert
            Assert.AreEqual(value, SecurePlayerPrefs.GetString("TestString" + value), "The value was not set");

            // Cleanup
            PlayerPrefs.DeleteKey("TestString" + value);
        }

        [TestCase("Test String 1")]
        [TestCase("Another Test")]
        [TestCase("Third Test")]
        public void SetString(string value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = false;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            SecurePlayerPrefs.DeleteKey("TestString" + value);
            SecurePlayerPrefs.SetString("TestString" + value, value);

            // Act

            // Assert
            Assert.AreEqual(value, PlayerPrefs.GetString("TestString" + value), "The value was not set");

            // Cleanup
            SecurePlayerPrefs.DeleteKey("TestString" + value);
        }

        [TestCase("Test String 1")]
        [TestCase("Another Test")]
        [TestCase("Third Test")]
        public void SetStringEncrypted(string value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            SecurePlayerPrefs.DeleteKey("TestString" + value);

            // Act
            SecurePlayerPrefs.SetString("TestString" + value, value);

            // Assert
            Assert.IsNotNull(SecurePlayerPrefs.GetRawEncryptedPrefsEntry("TestString" + value), "The value was null");
            Assert.AreNotEqual(value.ToString(), SecurePlayerPrefs.GetRawEncryptedPrefsEntry("TestString" + value), "The value was not set encrypted");

            // Cleanup
            SecurePlayerPrefs.DeleteKey("TestString" + value);
        }

        [TestCase("Test String 1")]
        [TestCase("Another Test")]
        [TestCase("Third Test")]
        public void GetStringEncrypted(string value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            SecurePlayerPrefs.DeleteKey("TestString" + value);

            // Act
            SecurePlayerPrefs.SetString("TestString" + value, value);

            // Assert
            Assert.AreEqual(value, SecurePlayerPrefs.GetString("TestString" + value), "The value could not be decrypted");

            // Cleanup
            SecurePlayerPrefs.DeleteKey("TestString" + value);
        }


        [TestCase("Test String 1")]
        [TestCase("Another Test")]
        [TestCase("Third Test")]
        public void GetStringAutoConvert(string value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = true;
            SecurePlayerPrefs.DeleteKey("TestString" + value);
            PlayerPrefs.DeleteKey("TestString" + value);
            PlayerPrefs.SetString("TestString" + value, value);

            // Act

            // Assert
            Assert.AreEqual(value, SecurePlayerPrefs.GetString("TestString" + value), "The value could not be decrypted");
            Assert.IsFalse(PlayerPrefs.HasKey("TestString" + value), "The old converted key was not removed");

            // Cleanup
            SecurePlayerPrefs.DeleteKey("TestString" + value);
            PlayerPrefs.DeleteKey("TestString" + value);
        }

        [TestCase("Test String 1")]
        [TestCase("Another Test")]
        [TestCase("Third Test")]
        public void GetStringNoAutoConvert(string value)
        {
            // Arrange
            SecurePlayerPrefs.UseSecurePrefs = true;
            SecurePlayerPrefs.AutoConvertUnsecurePrefs = false;
            SecurePlayerPrefs.DeleteKey("TestString" + value);
            PlayerPrefs.DeleteKey("TestString" + value);
            PlayerPrefs.SetString("TestString" + value, value);

            // Act

            // Assert
            Assert.AreNotEqual(value, SecurePlayerPrefs.GetString("TestString" + value), "The value was wrongly retrieved");
            Assert.IsTrue(PlayerPrefs.HasKey("TestString" + value), "The old converted key was removed");

            // Cleanup
            SecurePlayerPrefs.DeleteKey("TestString" + value);
            PlayerPrefs.DeleteKey("TestString" + value);
        }

        #endregion String Prefs Tests

    }
}
#endif