import { useState } from 'react'

export default function useController(initialValue = '') {
	const [value, setValue] = useState(initialValue)

	return {
		value,
		props: {
			value,
			onChange: (e: React.ChangeEvent<HTMLInputElement>) => setValue(e.target.value),
		},
	}
}
