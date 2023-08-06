# Description: This file contains the functions that are used to run the API
import base64
import os
import random
import json
import subprocess
import asyncio
import aiohttp

CONTRACTS_FILE_LOCATION = '..\\odkazySmluv.txt'
CONTRACTS_COUNT = 2
PATH_TO_API = 'API2/bin/Debug/net7.0/API2.exe'
URL = 'http://localhost:5000/analyze'

def create_post_request(url: str, fileLocation: str, returnImages: bool = False):
	"""
		Creates a post request that will be sent to the API
	"""
	headers = {'Content-Type': 'application/json'}
	data = {
		"fileLocation": fileLocation,
		"returnImages": returnImages
	}
	return url, headers, data


async def send_post_request(session, url: str, fileLocation: str, returnImages: bool = False):
	"""
		Sends a post request to the API asynchronously
	"""
	url, headers, data = create_post_request(url, fileLocation, returnImages)
	async with session.post(url, headers=headers, data=json.dumps(data)) as response:
		data = await response.text()
		return response.status, json.loads(data)

async def main(contractCount: int = 5):
	with open(CONTRACTS_FILE_LOCATION, 'r') as f:
		contracts = f.readlines()

	subprocess.Popen(PATH_TO_API)
	print('API started')

	contracts = [contract.strip()[1:-1] for contract in contracts if contract.strip() != '']

	contracts = random.sample(contracts, contractCount)

	async with aiohttp.ClientSession() as session:
		tasks = []
		for contract in contracts:
			print(contract)
			task = send_post_request(session, URL, contract, True)
			tasks.append(task)
		print('Sending requests')
		responses = await asyncio.gather(*tasks)

	for status, data in responses:
		print("Saving " + data['contractName'])
		# Print the data
		# if values in returnImages are True, the images will be returned
		# Save the images to the folder
		# for each contract, a folder will be created
		# the folder will be named after the contract
		# the images will be named after the page number
		folderName = data['contractName']
		# create the folder if it does not exist
		if not os.path.exists("analyzedImages\\" + folderName):
			os.makedirs("analyzedImages\\" + folderName)
		# save the images
		if data['resultImages']:
			images = data['resultImages']
			originalImages = data['originalImages']
			for i in range(len(images)):
					# for name of the folder, remove the path and the extension
					image = images[str(i+1)]
					originalImage = originalImages[str(i+1)]
					imageName = str(i) + '.jpg'
					originalImageName = str(i) + '_original.jpg'
					img_bytes = base64.b64decode(image)
					originalImg_bytes = base64.b64decode(originalImage)
					with open("analyzedImages\\" + folderName + '\\' + imageName, 'wb') as f:
						f.write(img_bytes)

					with open("analyzedImages\\" + folderName + '\\' + originalImageName, 'wb') as f:
						f.write(originalImg_bytes)
			# Save the rest of the data to a json file
		data.pop('resultImages', None)
		data.pop('originalImages', None)
		# create file 
		with open("analyzedImages\\" + folderName + '\\' + folderName + '.json', 'w') as f:
			json.dump(data, f, indent=4)

if __name__ == '__main__':
	try:
		loop = asyncio.get_event_loop()
		loop.run_until_complete(main(CONTRACTS_COUNT))
	except Exception as e:
		print(e)
	finally:
		subprocess.run('taskkill /f /im API2.exe')
		print('API stopped')