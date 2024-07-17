import { Schema } from '@/api/client'
import { useGetQuery } from '@/api/hooks'
import { queryClient } from '@/main'

export default function useUser() {
	const { data, error, ...params } = useGetQuery('/Auth/me', {})
	return { user: error ? null : data, ...params }
}

useUser.setData = (data: Schema<'AuthResponse'>['user'] | null) =>
	queryClient.setQueryData(['/Auth/me', {}], data)

useUser.invalidate = () => queryClient.invalidateQueries({ queryKey: ['/Auth/me'] })
