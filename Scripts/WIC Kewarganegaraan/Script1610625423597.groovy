import static com.kms.katalon.core.checkpoint.CheckpointFactory.findCheckpoint
import static com.kms.katalon.core.testcase.TestCaseFactory.findTestCase
import static com.kms.katalon.core.testdata.TestDataFactory.findTestData
import static com.kms.katalon.core.testobject.ObjectRepository.findTestObject
import static com.kms.katalon.core.testobject.ObjectRepository.findWindowsObject
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

WebUI.click(findTestObject('WIC/Page_/triger'))

WebUI.delay(10)

WebUI.click(findTestObject('WIC/Page_/Input Triger'))

WebUI.setText(findTestObject('WIC/Page_/Input Triger'), 'id')

WebUI.delay(10)

WebUI.click(findTestObject('WIC/Page_/OK Kwn 1'))

