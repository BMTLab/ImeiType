using System.Text;

using BMTLab.ImeiType;

using static System.Console;

var imei1 = new Imei("356303489916807"u8);
var imei2 = new Imei("356303489916807");
var imei3 = new Imei("356303489916807".ToArray());
var imei4 = new Imei("356303489916807".AsSpan());
var imei5 = new Imei(356303489916807);
var imei6 = new Imei([0x33, 0x35, 0x36, 0x33, 0x30, 0x33, 0x34, 0x38, 0x39, 0x39, 0x31, 0x36, 0x38, 0x30, 0x37]);
var imei7 = new Imei(['3', '5', '6', '3', '0', '3', '4', '8', '9', '9', '1', '6', '8', '0', '7']);

/* > Imei
   ╭────────┬─────────────────╮
   │ Name   │ Value           │
   ├────────┼─────────────────┤
   │ Tac    │ 35              │
   │ Fac    │ 630348          │
   │ Snr    │ 991680          │
   │ _value │ 356303489916807 │
   ╰────────┴─────────────────╯
 */

/* IMEI operator conversion */
WriteLine($"IMEI as long: {imei1}");                                        //> 356303489916807
WriteLine($"IMEI as long: {(long) imei2}");                                 //> 356303489916807
WriteLine($"IMEI as string (explicit): {(string) imei3}");                  //> 356303489916807
WriteLine($"IMEI as string (implicit): {imei4}");                           //> 356303489916807
WriteLine($"IMEI as string (via ToString()): {imei5.ToString()}");          //> 356303489916807
WriteLine($"IMEI as char array: {new string(imei6.ToReadOnlySpan())}");     //> 356303489916807
WriteLine($"IMEI as UTF8 text: {Encoding.UTF8.GetString(imei7.ToUtf8())}"); //> 356303489916807

/* IMEI's parts */
WriteLine($"IMEI's TAC: {imei1.Tac}"); //> 35
WriteLine($"IMEI's FAC: {imei1.Fac}"); //> 630348
WriteLine($"IMEI's SNR: {imei1.Snr}"); //> 991680

/* Generating a new IMEI number */
var newImei1 = Imei.NewRandomImei(seed: 42);
var newImei2 = Imei.NewRandomImei();
WriteLine($"Random new IMEI (with seed): {newImei1}");
WriteLine($"Random new IMEI (secure): {newImei2}");