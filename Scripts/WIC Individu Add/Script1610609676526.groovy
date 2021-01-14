import com.kms.katalon.core.checkpoint.Checkpoint as Checkpoint
import com.kms.katalon.core.cucumber.keyword.CucumberBuiltinKeywords as CucumberKW
import com.kms.katalon.core.mobile.keyword.MobileBuiltInKeywords as Mobile
import com.kms.katalon.core.model.FailureHandling as FailureHandling
import com.kms.katalon.core.testcase.TestCase as TestCase
import com.kms.katalon.core.testdata.TestData as TestData
import com.kms.katalon.core.testobject.TestObject as TestObject
import com.kms.katalon.core.webservice.keyword.WSBuiltInKeywords as WS
import com.kms.katalon.core.webui.keyword.WebUiBuiltInKeywords as WebUI
import com.kms.katalon.core.windows.keyword.WindowsBuiltinKeywords as Windows
import internal.GlobalVariable as GlobalVariable
import static com.kms.katalon.core.checkpoint.CheckpointFactory.findCheckpoint
import static com.kms.katalon.core.testcase.TestCaseFactory.findTestCase
import static com.kms.katalon.core.testdata.TestDataFactory.findTestData
import static com.kms.katalon.core.testobject.ObjectRepository.findTestObject
import static com.kms.katalon.core.testobject.ObjectRepository.findWindowsObject
import org.openqa.selenium.Keys as Keys
import com.kms.katalon.core.testng.keyword.TestNGBuiltinKeywords as TestNGKW

'Buka Browser\r\n'
WebUI.openBrowser('http://goaml.southeastasia.cloudapp.azure.com/goaml/Default.aspx')

'Maksimal Windows\r\n'
WebUI.maximizeWindow()

WebUI.refresh()

WebUI.setText(findTestObject('All/UserName'), 'sysadmin')

WebUI.setEncryptedText(findTestObject('All/password'), 'h1bZaoNZrXJGImBBMGIoZQ==')

WebUI.click(findTestObject('All/Sign In'))

WebUI.refresh()

WebUI.delay(3)

WebUI.setText(findTestObject('All/filter'), 'WIC')

WebUI.delay(1)

WebUI.click(findTestObject('WIC/View'))

WebUI.delay(3)

WebUI.click(findTestObject('All/New Record'))

WebUI.delay(3)

WebUI.setText(findTestObject('WIC/Individu/01/01 WIC No'), '12133746')

WebUI.delay(1)

WebUI.setText(findTestObject('WIC/Individu/01/02 Jenis Kelamin'), 'female')

WebUI.delay(1)

WebUI.sendKeys(findTestObject('WIC/Individu/01/02 Jenis Kelamin'), Keys.chord(Keys.ENTER))

WebUI.setText(findTestObject('WIC/Individu/01/03 Gelar'), 'mr')

WebUI.setText(findTestObject('WIC/Individu/01/04 Nama Lengkap'), 'anggi firmansah')

WebUI.setText(findTestObject('WIC/Individu/01/05 Tanggal Lahir'), '9/15/1998')

WebUI.delay(1)

WebUI.setText(findTestObject('WIC/Individu/01/06 Tempat Lahir'), 'jakarta')

WebUI.click(findTestObject('WIC/Page_/triger'))

WebUI.delay(1)

WebUI.click(findTestObject('WIC/Page_/Input Triger'))

WebUI.setText(findTestObject('WIC/Page_/Input Triger'), 'id')

WebUI.delay(1)

WebUI.click(findTestObject('WIC/Page_/isi'))

WebUI.click(findTestObject('WIC/Individu/02/01 New Phone Detail'))

WebUI.setText(findTestObject('WIC/Individu/02/02 Kategori Kontak'), 'Domisili')

WebUI.delay(1)

WebUI.setText(findTestObject('WIC/Individu/02/02 Kategori Kontak'), Keys.chord(Keys.ENTER))

WebUI.setText(findTestObject('WIC/Individu/02/03 Jenis Alat Komunikasi'), 'Satelit')

WebUI.delay(1)

WebUI.setText(findTestObject('WIC/Individu/02/03 Jenis Alat Komunikasi'), Keys.chord(Keys.ENTER))

WebUI.setText(findTestObject('WIC/Individu/02/05 No Telepon'), '0827367623')

WebUI.click(findTestObject('WIC/Individu/02/08 Save'))

WebUI.scrollToElement(findTestObject('WIC/Page_/negara domisili'), 2)

WebUI.click(findTestObject('WIC/Page_/negara domisili'), FailureHandling.STOP_ON_FAILURE)

WebUI.delay(1)

WebUI.click(findTestObject('WIC/Page_/isi - Copy'))

