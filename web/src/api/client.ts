import createClient, { Middleware } from 'openapi-fetch'
import type { paths, components } from './schema'

const authMiddleware: Middleware = {
	onRequest({ request }) {
		const token = localStorage.getItem('token')
		if (token) {
			request.headers.set('Authorization', `Bearer ${token}`)
		}
		return request
	},
}

export const api = createClient<paths>({ baseUrl: import.meta.env.VITE_API_URL as string })
api.use(authMiddleware)

export type Schema<K extends keyof components['schemas']> = components['schemas'][K]

type TuplifyUnion<U extends string, R extends unknown[] = []> = {
	[S in U]: Exclude<U, S> extends never ? [...R, S] : TuplifyUnion<Exclude<U, S>, [...R, S]>
}[U]

export const categoryTypes: TuplifyUnion<Schema<'ContactCategory'>['type']> = [
	'PERSONAL',
	'WORK',
	'OTHER',
] as const

export function translateCategoryType(type: Schema<'ContactCategory'>['type']) {
	switch (type) {
		case 'PERSONAL':
			return 'Osobisty'
		case 'WORK':
			return 'Służbowy'
		default:
			return 'Inny'
	}
}
